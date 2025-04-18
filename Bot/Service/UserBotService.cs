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

    #region Обработка сообщений

    public async Task HandleMessageAsync(Message message, CancellationToken cancellationToken)
    {
        if (message.Contact != null)
        {
            await HandlePhoneNumberAsync(message, cancellationToken);
            return;
        }

        if (message.Text is null) return;

        var chatId = message.Chat.Id;
        var messageText = message.Text;

        _logger.LogInformation($"Получено сообщение от {chatId}: {messageText}");

        var session = _sessionService.GetOrCreate(chatId);

        // FSM (Finite State Machine)
        switch (session.State)
        {
            case UserState.AwaitingFullName:
                await ProcessFullNameAsync(chatId, messageText, cancellationToken);
                return;
        }

        // Команды
        switch (messageText)
        {
            case "/start":
                await ProcessStartCommandAsync(chatId, cancellationToken);
                break;

            default:
                await SendUnknownCommandResponse(chatId, cancellationToken);
                break;
        }
    }

    public Task HandleCallbackQueryAsync(CallbackQuery callbackQuery, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    #endregion

    #region FSM: Обработка состояний

    private async Task ProcessFullNameAsync(long chatId, string fullName, CancellationToken cancellationToken)
    {
        var session = _sessionService.GetOrCreate(chatId);
        session.FullName = fullName;
        _sessionService.SetState(chatId, UserState.AwaitingPhone);

        await RequestPhoneNumber(chatId, cancellationToken);
    }

    #endregion

    #region Обработка команд

    private async Task ProcessStartCommandAsync(long chatId, CancellationToken cancellationToken)
    {
        if (await _userRepository.IsAuthorizedAsync(chatId))
        {
            await _botClient.SendTextMessageAsync(chatId, "🎉 Вы уже авторизованы! Можно продолжать обучение.", cancellationToken: cancellationToken);
            return;
        }

        await SendGreeting(chatId, cancellationToken);

        var session = _sessionService.GetOrCreate(chatId);
        session.State = UserState.AwaitingFullName;

        await AskFullNameAsync(chatId, cancellationToken);
    }

    private async Task SendUnknownCommandResponse(long chatId, CancellationToken cancellationToken)
    {
        await _botClient.SendTextMessageAsync(
            chatId,
            "🤔 Не понимаю. Используйте команду /start для начала.",
            cancellationToken: cancellationToken);
    }

    #endregion

    #region Telegram UI

    private async Task SendGreeting(long chatId, CancellationToken cancellationToken)
    {
        await _botClient.SendTextMessageAsync(chatId, "Йоу, салам 👋", cancellationToken: cancellationToken);
    }

    private async Task AskFullNameAsync(long chatId, CancellationToken cancellationToken)
    {
        await _botClient.SendTextMessageAsync(
            chatId,
            "✍️ Введите свои ФИО (например: Иванов Иван Иванович):",
            cancellationToken: cancellationToken);
    }

    private async Task RequestPhoneNumber(long chatId, CancellationToken cancellationToken)
    {
        var keyboard = new ReplyKeyboardMarkup(new[]
        {
            new[]
            {
                KeyboardButton.WithRequestContact("📱 Отправить номер телефона")
            }
        })
        {
            ResizeKeyboard = true,
            OneTimeKeyboard = true
        };

        await _botClient.SendTextMessageAsync(
            chatId: chatId,
            text: "Отлично! Теперь отправьте номер телефона, используя кнопку ниже:",
            replyMarkup: keyboard,
            cancellationToken: cancellationToken);
    }

    #endregion

    #region Обработка контакта и завершение авторизации

    private async Task HandlePhoneNumberAsync(Message message, CancellationToken cancellationToken)
    {
        var chatId = message.Chat.Id;
        var session = _sessionService.GetOrCreate(chatId);

        session.PhoneNumber = message.Contact.PhoneNumber;
        session.State = UserState.Authorized;

        await _userRepository.SaveAsync(chatId, session.FullName, session.PhoneNumber);
        _sessionService.Clear(chatId);

        await _botClient.SendTextMessageAsync(
            chatId: chatId,
            text: $"✅ Спасибо, {session.FullName}! Вы авторизованы.\nВаш номер: {session.PhoneNumber}",
            replyMarkup: new ReplyKeyboardRemove(),
            cancellationToken: cancellationToken);
    }

    #endregion
}
