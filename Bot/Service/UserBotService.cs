using System.Collections.Concurrent;
using Bot.Helpers.Session;
using Bot.Helpers.Session.Interface;
using Bot.Service.Interfaces;
using Core.Entity;
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
    private readonly ICourseRepository _courseRepository;
    private readonly ITopicRepository _topicRepository;
    private readonly ILogger<UserBotService> _logger;
    private readonly IUserSessionService _sessionService;
    
    // нажми кнопку снизу, если готов пройти тест 
    // это нужно выдавать в keyboard button - после темы
    // после каждой темы - должен быть тест на один вопрос

    public UserBotService(
        ITelegramBotClient botClient,
        IUserRepository userRepository,
        ICourseRepository courseRepository,
        ITopicRepository topicRepository,
        ILogger<UserBotService> logger,
        IUserSessionService sessionService)
    {
        _botClient = botClient;
        _userRepository = userRepository;
        _courseRepository = courseRepository;
        _topicRepository = topicRepository;
        _logger = logger;
        _sessionService = sessionService;
    }
    
    public async Task HandleMessageAsync(Message message, CancellationToken cancellationToken)
    {
        if (message.Text is null && message.Contact is null) // если юзер не отправил ни сообщение ни контакт, то ингорим
            return;
        
        var chatId = message.Chat.Id;
        var messageText = message.Text;
        
        
        _logger.LogInformation($"Получено сообщение от {chatId}: {messageText}");
        
        // 1. Обрабатываем команду (в том числе /start) — всегда!
        var shouldStop = await HandleUserStateAsync(chatId, message, cancellationToken);
        if (shouldStop)
            return;
        
        // 2. Обрабатываем команды
       shouldStop = await HandleCommandAsync(chatId, messageText, cancellationToken);
       if (shouldStop)
           return;
        
        // 3. Если команда не остановила, смотрим авторизацию и показываем главное меню
        if (await _userRepository.IsAuthorizedAsync(chatId))
        {
            await ShowWelcomeMenuAsync(chatId, cancellationToken);
            return;
        }
    }
    
    private async Task ShowWelcomeMenuAsync(long chatId, CancellationToken cancellationToken)
    {
        var keyboard = new InlineKeyboardMarkup(new[]
        {
            new[]
            {
                InlineKeyboardButton.WithCallbackData("📚 Темы", "topics"),
                InlineKeyboardButton.WithCallbackData("❓ Вопросы", "faq")
            },
            new[]
            {
                InlineKeyboardButton.WithCallbackData("🛠 Поддержка", "support")
            }
        });

        await _botClient.SendTextMessageAsync(
            chatId,
            "Выберите интересующий раздел:",
            replyMarkup: keyboard,
            cancellationToken: cancellationToken);
    }
    
    public async Task HandleCallbackQueryAsync(CallbackQuery callbackQuery, CancellationToken cancellationToken)
    {
        var chatId = callbackQuery.Message.Chat.Id;
        var data = callbackQuery.Data;

        switch (data)
        {
            case "topics":
                // Загружаем список тем из репозитория
                var course = await _courseRepository.GetByIdWithTopicsAsync(1); // у нас всего один курс ;)
                if (course == null)
                {
                    await _botClient.SendTextMessageAsync(chatId, "❌ Курс не найден. Попробуйте позже.", cancellationToken: cancellationToken);
                    break;
                }

                var topicButtons = course.Topics
                    .Select((topic, index) => new[]
                    {
                        InlineKeyboardButton.WithCallbackData($"{index + 1}. {topic.Title}", $"topic_{topic.Id}")
                    })
                    .ToArray();

                var keyboard = new InlineKeyboardMarkup(topicButtons);


                // Отправляем пользователю список тем
                await _botClient.SendTextMessageAsync(
                    chatId,
                    "📚 Список тем:\n" + string.Join("\n", course.Topics.Select(t => t.Title)),
                    replyMarkup: keyboard,
                    cancellationToken: cancellationToken);
                break;

            case "support":
                // Информация по техподдержке
                await _botClient.SendTextMessageAsync(
                    chatId,
                    "🛠 Связь с техподдержкой:\nНапишите ваш вопрос сюда - @BarsBotHelper, и мы постараемся помочь.",
                    cancellationToken: cancellationToken);
                break;

            default:
                if (data.StartsWith("topic_"))
                {
                    // Показ подробностей по выбранной теме
                    var topicId = int.Parse(data.Split('_')[1]);
                    await ShowTopicDetailsAsync(chatId, topicId, cancellationToken);
                }
                break;
            
            case "faq":
                var faqText = "❓ *Часто задаваемые вопросы:*\n\n" +
                              "1️⃣ *Как проходит обучение?*\n" +
                              "Обучение проходит онлайн, с доступом к материалам через этого бота.\n\n" +
                              "2️⃣ *Что делать, если возникли проблемы?*\n" +
                              "Обратитесь в техподдержку через раздел 🛠 Поддержка.\n\n" +
                              "3️⃣ *Можно ли проходить курс в удобное время?*\n" +
                              "Да! Вы можете проходить темы тогда, когда вам удобно.";

                await _botClient.SendTextMessageAsync(
                    chatId,
                    faqText,
                    parseMode: Telegram.Bot.Types.Enums.ParseMode.Markdown,
                    cancellationToken: cancellationToken);
                break;
        }
        
        // Удаляем кнопку после нажатия (если нужно)
        await _botClient.AnswerCallbackQueryAsync(callbackQuery.Id, cancellationToken: cancellationToken);
    }
    
    private async Task ShowTopicDetailsAsync(long chatId, int topicId, CancellationToken cancellationToken)
    {
        // Загружаем тему по её ID из репозитория
        var topic = await _topicRepository.GetByIdAsync(topicId);
    
        if (topic == null)
        {
            // Если тема не найдена, отправляем сообщение об ошибке
            await _botClient.SendTextMessageAsync(
                chatId,
                "❌ Тема не найдена. Попробуйте снова.",
                cancellationToken: cancellationToken);
            return;
        }

        // Заголовок темы
        await _botClient.SendTextMessageAsync(
            chatId,
            $"📚 *{topic.Title}*",
            parseMode: Telegram.Bot.Types.Enums.ParseMode.Markdown,
            cancellationToken: cancellationToken);

        foreach (var contentItem in topic.ContentItems)
        {
            switch (contentItem)
            {
                case TextContent text:
                    await _botClient.SendTextMessageAsync(
                        chatId,
                        $"📝 *{text.Title}*\n_{text.Description}_\n\n{text.Text}",
                        parseMode: Telegram.Bot.Types.Enums.ParseMode.Markdown,
                        cancellationToken: cancellationToken);
                    break;

                // case VideoContent video:
                //     await _botClient.SendVideoAsync(
                //         chatId,
                //         video.VideoUrl,
                //         caption: $"🎥 *{video.Title}*\n_{video.Description}_\nДлительность: {video.Duration:mm\\:ss}",
                //         parseMode: Telegram.Bot.Types.Enums.ParseMode.Markdown,
                //         thumbnail: string.IsNullOrWhiteSpace(video.ThumbnailUrl)
                //             ? null
                //             : InputFile.FromUri(video.ThumbnailUrl),
                //         cancellationToken: cancellationToken);
                //     break;
                //
                // case ImageContent image:
                //     await _botClient.SendPhotoAsync(
                //         chatId,
                //         image.ImageUrl,
                //         caption: $"🖼 *{image.Title}*\n_{image.Description}_",
                //         parseMode: Telegram.Bot.Types.Enums.ParseMode.Markdown,
                //         cancellationToken: cancellationToken);
                //     break;
                //
                // case LinkContent link:
                //     await _botClient.SendTextMessageAsync(
                //         chatId,
                //         $"🔗 *{link.Title}*\n_{link.Description}_\n[Перейти по ссылке]({link.Url})",
                //         parseMode: Telegram.Bot.Types.Enums.ParseMode.Markdown,
                //         cancellationToken: cancellationToken);
                //     break;
            }
        }
    }
    
    private async Task<bool> HandleUserStateAsync(long chatId, Message message, CancellationToken cancellationToken)
    {
        var messageText = message.Text;
            
        if (!string.IsNullOrEmpty(message.Text) && message.Text.StartsWith("/"))
        {
            _sessionService.Clear(chatId); // Сброс состояния
            return false;
        }
        
        if (messageText == "❌ Отменить регистрацию")
        {
            _sessionService.Clear(chatId);

            await _botClient.SendTextMessageAsync(
                chatId,
                "🚫 Регистрация отменена.",
                replyMarkup: new ReplyKeyboardRemove(),
                cancellationToken: cancellationToken);
            return true;
            // отправить главное меню
            await ShowWelcomeMenuAsync(chatId, cancellationToken);
        }
        
        var session = _sessionService.GetOrCreate(chatId);
        
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
        
        // выставляем новое состояние - ждем ФИО
        session.State = UserState.AwaitingFullName;
        
        await _botClient.SendTextMessageAsync(chatId,
            "✍️ Введите свои ФИО (например: Иванов Иван Иванович):",
            replyMarkup: new ReplyKeyboardMarkup(new[]
            {
                new[] { new KeyboardButton("❌ Отменить регистрацию") }
            })
            {
                ResizeKeyboard = true,
                OneTimeKeyboard = true
            },
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
            new[]
            {
                KeyboardButton.WithRequestContact("📱 Отправить номер телефона")
            },
            new[]
            {
                new KeyboardButton("❌ Отменить регистрацию")
            }
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
    
    private async Task<bool> HandleCommandAsync(long chatId, string messageText, CancellationToken cancellationToken)
    {
        if (!messageText.StartsWith("/"))
            return false; // не команда
        
        switch (messageText)
        {
            case "/start":
                _sessionService.Clear(chatId); // сбрасываем старое состояние
                var session = _sessionService.GetOrCreate(chatId); // получаем новую пустую сессию
                await ProcessStartCommandAsync(chatId, session, cancellationToken);
                return true;
            
            default:
                await _botClient.SendTextMessageAsync(
                    chatId,
                    "🤔 Не понимаю эту команду. Попробуйте /start.",
                    cancellationToken: cancellationToken);
                return true;
        }
    }
    
    #region helpers

    private bool IsValidFullName(string fullName)
    {
        return fullName.Trim().Split(' ').Length >= 2 && fullName.Length > 5;
    }

    #endregion
}
