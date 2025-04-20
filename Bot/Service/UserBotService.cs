using System.Collections.Concurrent;
using Bot.Helpers.Session;
using Bot.Helpers.Session.Interface;
using Bot.Service.Interfaces;
using Core.Interfaces;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace Bot.Service;

public class UserBotService : IUserBotService
{
    private readonly ITelegramBotClient _botClient;
    private readonly IUserRepository _userRepository;
    private readonly ILogger<UserBotService> _logger;
    private readonly IUserSessionService _sessionService;

    public UserBotService(
        ITelegramBotClient botClient,
        IUserRepository userRepository,
        ILogger<UserBotService> logger,
        IUserSessionService sessionService)
    {
        _botClient = botClient;
        _userRepository = userRepository;
        _logger = logger;
        _sessionService = sessionService;
    }

    
    public async Task HandleMessageAsync(Message message, CancellationToken cancellationToken)
    {
        if (message.Text is null && message.Contact is null) // если юзер не отправил ни сообщение ни контакт, то ингорим
            return;
        
        var chatId = message.Chat.Id;
        var messageText = message.Text;
        var session = _sessionService.GetOrCreate(chatId);
        
        _logger.LogInformation($"Получено сообщение от {chatId}: {messageText}");
        
        var shouldStop = await HandleUserStateAsync(chatId, session, message, cancellationToken);
        if (shouldStop)
            return;
        
        session = _sessionService.GetOrCreate(chatId);
        await HandleCommandAsync(chatId, session, messageText, cancellationToken);
    }

    public Task HandleCallbackQueryAsync(CallbackQuery callbackQuery, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    private async Task<bool> HandleUserStateAsync(long chatId, UserSession session, Message message, CancellationToken cancellationToken)
    {
        var messageText = message.Text;
            
        if (!string.IsNullOrEmpty(message.Text) && message.Text.StartsWith("/"))
        {
            _sessionService.Clear(chatId); // Сброс состояния
            return false;
        }
        
        switch (session.State)
        {
            case UserState.AwaitingFullName:
                // Проверка валидация ФИО
                if (string.IsNullOrEmpty(messageText) || !IsValidFullName(messageText))
                {
                    await _botClient.SendTextMessageAsync(
                        chatId, 
                        "❗Введите корректные данные ФИО", 
                        cancellationToken: cancellationToken);
                    return true;
                }
                
                session.FullName = messageText;
                session.State = UserState.AwaitingPhone;
                await RequestPhoneNumberAsync(chatId, cancellationToken);
                return true;
            
            case UserState.AwaitingPhone:
                // проверяем, отправил ли пользователь свой контакт
                if (message.Contact != null && message.Contact.UserId == message.From.Id)
                {
                    await HandlePhoneNumberAsync(message, cancellationToken);
                    return true;
                }   
                else
                {
                    // Если пользователь пытается ввести телефон вручную или отправил не свой контакт
                    await _botClient.SendTextMessageAsync(
                        chatId, 
                        "📱 Отправьте номер телефона через кнопку ниже 👇",
                        cancellationToken: cancellationToken);
                    return true;
                }
        }
        return false;
    }

    private async Task ProcessStartCommandAsync(long chatId, UserSession session, CancellationToken cancellationToken)
    {
        await _botClient.SendTextMessageAsync(chatId, "Привет, я бот от компании Bars групп, который будет обучать тебя на курсе 'Тим Лид' 👋", cancellationToken: cancellationToken);
        
        if (await _userRepository.IsAuthorizedAsync(chatId))
        {
            await _botClient.SendTextMessageAsync(
                chatId,
                "🎉 Вы уже авторизованы! Можно продолжать обучение.",
                cancellationToken: cancellationToken);
            return;
        }
        
        // выставляем новое состояние - ждем ФИО
        session.State = UserState.AwaitingFullName;
        
        await _botClient.SendTextMessageAsync(chatId,
            "✍️ Введите свои ФИО (например: Иванов Иван Иванович):",
            cancellationToken: cancellationToken);
    }

    private async Task HandlePhoneNumberAsync(Message message, CancellationToken cancellationToken)
    {
        var chatId = message.Chat.Id;
        var phoneNumber = message.Contact.PhoneNumber;

        var user = await _userRepository.GetByPhoneNumberAsync(phoneNumber);

        if (user is null)
        {
            await _botClient.SendTextMessageAsync(
                chatId,
                "🚫 Доступ запрещён. Ваш номер не найден в базе зарегистрированных пользователей.",
                replyMarkup: new ReplyKeyboardRemove(),
                cancellationToken: cancellationToken);

            _sessionService.Clear(chatId);
            return;
        }

        user.ChatId = chatId;
        user.LastActivity = DateTime.UtcNow;
        await _userRepository.UpdateAsync(user);

        _sessionService.Clear(chatId);

        await _botClient.SendTextMessageAsync(
            chatId,
            $"✅ Добро пожаловать, {user.FullName}!\nВы успешно авторизованы.",
            replyMarkup: new ReplyKeyboardRemove(),
            cancellationToken: cancellationToken);
    }

    private async Task RequestPhoneNumberAsync(long chatId, CancellationToken cancellationToken)
    {
        var keyboard = new ReplyKeyboardMarkup(new[]
        {
            new[] { KeyboardButton.WithRequestContact("📱 Отправить номер телефона") }
        })
        {
            ResizeKeyboard = true,
            OneTimeKeyboard = true
        };

        await _botClient.SendTextMessageAsync(
            chatId,
            "Отлично! Теперь отправьте номер телефона, используя кнопку ниже:",
            replyMarkup: keyboard,
            cancellationToken: cancellationToken);
    }
    
    private async Task HandleCommandAsync(long chatId, UserSession session, string messageText, CancellationToken cancellationToken)
    {
        switch (messageText)
        {
            case "/start":
                _sessionService.Clear(chatId); // сбрасываем старое состояние
                session = _sessionService.GetOrCreate(chatId); // получаем новую пустую сессию
                await ProcessStartCommandAsync(chatId, session, cancellationToken);
                break;

            default:
                await _botClient.SendTextMessageAsync(
                    chatId,
                    "🤔 Не понимаю. Используйте команду /start для начала.",
                    cancellationToken: cancellationToken);
                break;
        }
    }

    
    #region helpers

    private bool IsValidFullName(string fullName)
    {
        return fullName.Trim().Split(' ').Length >= 2 && fullName.Length > 5;
    }

    #endregion
}
