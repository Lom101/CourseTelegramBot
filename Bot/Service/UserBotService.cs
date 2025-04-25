using Bot.Helpers.TestSession.Interface;
using Bot.Helpers.UserSession;
using Bot.Helpers.UserSession.Interface;
using Bot.Service.Interfaces;
using Core.Entity.Test;
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
    private readonly IBlockRepository _blockRepository;
    private readonly ITopicRepository _topicRepository;
    private readonly IUserProgressRepository _userProgressRepository;
    private readonly IFinalTestRepository _testRepository;
    private readonly ILogger<UserBotService> _logger;
    private readonly IUserSessionService _userSessionService;
    private readonly IFinalTestSessionService _testSessionService;

    public UserBotService(
        ITelegramBotClient botClient,
        IUserRepository userRepository,
        IBlockRepository blockRepository,
        ITopicRepository topicRepository,
        IUserProgressRepository userProgressRepository,
        IFinalTestRepository testRepository,
        ILogger<UserBotService> logger,
        IUserSessionService userSessionService,
        IFinalTestSessionService testSessionService)
    {
        _botClient = botClient;
        _userRepository = userRepository;
        _blockRepository = blockRepository;
        _topicRepository = topicRepository;
        _userProgressRepository = userProgressRepository;
        _testRepository = testRepository;
        _logger = logger;
        _userSessionService = userSessionService;
        _testSessionService = testSessionService; 
    }
    
    # region Generic
    
    public async Task HandleMessageAsync(Message message, CancellationToken cancellationToken)
    {
        if (message.Text is null && message.Contact is null) // если юзер не отправил ни сообщение ни контакт, то ингорим
            return;
        
        var chatId = message.Chat.Id;
        var messageText = message.Text;
        
        
        _logger.LogInformation($"Получено сообщение от {chatId}: {messageText}");
        
        // 1. Обрабатываем состояние, и сброс, если получили команду
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
        }
    }
    
    private async Task<bool> HandleUserStateAsync(long chatId, Message message, CancellationToken cancellationToken)
    {
        var messageText = message.Text;
            
        if (!string.IsNullOrEmpty(message.Text) && message.Text.StartsWith("/"))
        {
            _userSessionService.Clear(chatId); // Сброс состояния
            return false;
        }
        
        if (messageText == "❌ Отменить регистрацию")
        {
            _userSessionService.Clear(chatId);

            await _botClient.SendTextMessageAsync(
                chatId,
                "🚫 Регистрация отменена.",
                replyMarkup: new ReplyKeyboardRemove(),
                cancellationToken: cancellationToken);
            if (await _userRepository.IsAuthorizedAsync(chatId))
            {
                ShowWelcomeMenuAsync(chatId, cancellationToken);
            }
            return true;
        }
        
        var session = _userSessionService.GetOrCreate(chatId);
        
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
                    await HandlePhoneNumberAsync(message, session, cancellationToken);
                    return true;
                }

                // Если пользователь пытается ввести телефон вручную или отправил не свой контакт
                await _botClient.SendTextMessageAsync(
                    chatId, 
                    "📱 Отправьте номер телефона через кнопку ниже 👇",
                    cancellationToken: cancellationToken);
                return true;
        }
        return false;
    }
    
    private async Task<bool> HandleCommandAsync(long chatId, string messageText, CancellationToken cancellationToken)
    {
        if (!messageText.StartsWith("/"))
            return false; // не команда
        
        switch (messageText)
        {
            case "/start":
                _userSessionService.Clear(chatId); // сбрасываем старое состояние
                var session = _userSessionService.GetOrCreate(chatId); // получаем новую пустую сессию
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
    
    public async Task HandleCallbackQueryAsync(CallbackQuery callbackQuery, CancellationToken cancellationToken)
    {
        var chatId = callbackQuery.Message.Chat.Id;
        var messageId = callbackQuery.Message.MessageId;
        var data = callbackQuery.Data;

        switch (data)
        {
            case "blocks":
                await ShowBlocksAsync(chatId, cancellationToken);
                break;

            case var block when block.StartsWith("block_"):
                var blockId = int.Parse(block.Split('_')[1]);
                await ShowTopicsAsync(chatId, blockId, cancellationToken);
                break;
            
            case var topic when topic.StartsWith("topic_completed_"):
                var topicId = int.Parse(topic.Split('_')[2]);
                await UpdateTopicProgress(chatId, topicId, cancellationToken);
                break;

            case var topic when topic.StartsWith("topic_"):
                 topicId = int.Parse(topic.Split('_')[1]);
                await ShowTopicDetailsAsync(chatId, topicId, cancellationToken);
                break;
            
            case var test when test.StartsWith("test_"):
                var blockIdForTest = int.Parse(test.Split('_')[1]);
                await HandleTestAsync(chatId, blockIdForTest, cancellationToken); // добавляем обработку теста
                break;
            
            case var answer when answer.StartsWith("answer_"):
                var selectedIndex = int.Parse(answer.Split('_')[1]);
                await HandleAnswerAsync(chatId, selectedIndex, cancellationToken); // обработка ответа на вопрос
                break;

            
            case "support":
                await ShowSupportInfoAsync(chatId, cancellationToken);
                break;

            case "faq":
                await ShowFaqInfoAsync(chatId, cancellationToken);
                break;
        }
        
        //await _botClient.DeleteMessageAsync(chatId, messageId);
        await _botClient.AnswerCallbackQueryAsync(callbackQuery.Id, cancellationToken: cancellationToken);
    }
    
    # endregion
    
    # region test session
    
    // код работы с тестами
    private async Task HandleTestAsync(long chatId, int blockId, CancellationToken cancellationToken)
    {
        var test = await _testRepository.GetByBlockIdAsync(blockId);
        if (test == null)
        {
            await _botClient.SendTextMessageAsync(chatId, "❌ Тест для этого блока не найден.", cancellationToken: cancellationToken);
            return;
        }

        // Начинаем сессию теста
        _testSessionService.StartSession(chatId, test);

        var currentQuestion = test.Questions.First();
        await _botClient.SendTextMessageAsync(
            chatId,
            currentQuestion.QuestionText,
            replyMarkup: GenerateAnswerButtons(currentQuestion),
            cancellationToken: cancellationToken);
    }
    
    private async Task HandleAnswerAsync(long chatId, int selectedIndex, CancellationToken cancellationToken)
    {
        if (_testSessionService.TryGetSession(chatId, out var session))
        {
            var currentQuestion = session.Test.Questions[session.CurrentQuestionIndex];

            // Сохраняем ответ
            _testSessionService.SaveAnswer(chatId, selectedIndex);

            if (session.CurrentQuestionIndex >= session.Test.Questions.Count)
            {
                // Если тест завершен, сохраняем результаты в БД
                await _botClient.SendTextMessageAsync(
                    chatId,
                    "🎉 Поздравляем, тест завершен! Ваш результат: [тут может быть логика вывода результатов].",
                    cancellationToken: cancellationToken);

                // Закрыть сессию
                _testSessionService.ClearSession(chatId);
            }
            else
            {
                // Переходим к следующему вопросу
                var nextQuestion = session.Test.Questions[session.CurrentQuestionIndex];
                await _botClient.SendTextMessageAsync(
                    chatId,
                    nextQuestion.QuestionText,
                    replyMarkup: GenerateAnswerButtons(nextQuestion),
                    cancellationToken: cancellationToken);
            }
        }
    }
    
    private InlineKeyboardMarkup GenerateAnswerButtons(TestQuestion question)
    {
        var buttons = question.Options.Select((option, index) => InlineKeyboardButton.WithCallbackData(option.OptionText, $"answer_{index}")).ToArray();
        return new InlineKeyboardMarkup(buttons);
    }
    
    # endregion test session
    
    
    # region Handlers

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
    
    private async Task HandlePhoneNumberAsync(Message message, UserSession session, CancellationToken cancellationToken)
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

            _userSessionService.Clear(chatId);
            return;
        }

        user.ChatId = chatId;
        user.FullName = session.FullName;
        user.LastActivity = DateTime.UtcNow;
        await _userRepository.UpdateAsync(user);

        _userSessionService.Clear(chatId);

        await _botClient.SendTextMessageAsync(
            chatId,
            $"✅ Добро пожаловать, {user.FullName}!\nВы успешно авторизованы.",
            replyMarkup: new ReplyKeyboardRemove(),
            cancellationToken: cancellationToken);
        
        ShowWelcomeMenuAsync(chatId, cancellationToken);
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
    
    # endregion
    
    # region Send Messages
    
    private async Task ShowBlocksAsync(long chatId, CancellationToken cancellationToken)
    {
        var blocks = await _blockRepository.GetAllAsync();

        var buttons = new List<InlineKeyboardButton>();

        var user = await _userRepository.GetByChatIdAsync(chatId);
        var userId = user.Id;
        
        foreach (var block in blocks)
        {
            // Получаем прогресс пользователя по данному блоку
            var blockProgress = await _userProgressRepository.GetBlockCompletionProgressAsync(userId, block.Id);
            var blockStatus = blockProgress?.IsBlockCompleted == true ? "✅ Пройден" : "";

            // Добавляем кнопку с статусом
            buttons.Add(InlineKeyboardButton.WithCallbackData($"{block.Title} {blockStatus}", $"block_{block.Id}"));
        }

        var keyboard = new InlineKeyboardMarkup(buttons.Chunk(1));

        await _botClient.SendTextMessageAsync(
            chatId, 
            "📚 Выберите блок:",
            replyMarkup: keyboard,
            cancellationToken: cancellationToken);
    }
    
    private async Task ShowTopicsAsync(long chatId, int blockId, CancellationToken cancellationToken)
    {
        var topics = await _topicRepository.GetByBlockIdAsync(blockId);
        if (!topics.Any())
        {
            await _botClient.SendTextMessageAsync(
                chatId,
                "❌ В этом блоке нет тем.",
                cancellationToken: cancellationToken);
            return;
        }

        var buttons = new List<InlineKeyboardButton>();
        
        var user = await _userRepository.GetByChatIdAsync(chatId);
        var userId = user.Id;
    
        foreach (var topic in topics)
        {
            // Получаем прогресс пользователя по данному топику
            var topicProgress = await _userProgressRepository.GetTopicProgressAsync(userId, topic.Id);
            var topicStatus = topicProgress?.IsCompleted == true ? "✅ Пройден" : "";

            // Добавляем кнопку с статусом
            buttons.Add(InlineKeyboardButton.WithCallbackData($"{topic.Title} {topicStatus}", $"topic_{topic.Id}"));
        }

        var keyboard = new InlineKeyboardMarkup(buttons.Chunk(1));
        await _botClient.SendTextMessageAsync(
            chatId,
            "📖 Выберите тему:",
            replyMarkup: keyboard,
            cancellationToken: cancellationToken);
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
        
        var keyboard = new InlineKeyboardMarkup(InlineKeyboardButton.WithCallbackData("✅ Прочитал", $"topic_completed_{topicId}"));

        // Выводим заголовок темы и ссылку на Longread
        await _botClient.SendTextMessageAsync(
            chatId,
            $"📚 {topic.Title}\n Ссылка на урок - {topic.LongreadUrl ?? "пусто"}",
            replyMarkup: keyboard,
            cancellationToken: cancellationToken);
    }
    
    private async Task ShowWelcomeMenuAsync(long chatId, CancellationToken cancellationToken)
    {
        var keyboard = new InlineKeyboardMarkup(new[]
        {
            new[]
            {
                InlineKeyboardButton.WithCallbackData("📚 Начать обучение", "blocks"),
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
    
    private async Task ShowSupportInfoAsync(long chatId, CancellationToken cancellationToken)
    {
        await _botClient.SendTextMessageAsync(
            chatId,
            "🛠 Связь с техподдержкой:\nНапишите ваш вопрос сюда - @BarsBotHelper, и мы постараемся помочь.",
            cancellationToken: cancellationToken);
    }

    private async Task ShowFaqInfoAsync(long chatId, CancellationToken cancellationToken)
    {
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
    }

    
    # endregion
    
    #region helpers

    private bool IsValidFullName(string fullName)
    {
        return fullName.Trim().Split(' ').Length >= 2 && fullName.Length > 5;
    }
    
    private async Task UpdateTopicProgress(long chatId, int topicId, CancellationToken cancellationToken)
    {
        // ✅ Обновляем прогресс как пройденную тему
        var user = await _userRepository.GetByChatIdAsync(chatId);
        await _userProgressRepository.MarkTopicCompletedAsync(user.Id, topicId);

        // ⬇️ Проверка: последняя ли это тема?
        var topic = await _topicRepository.GetByIdAsync(topicId);
        var allTopics = await _topicRepository.GetByBlockIdAsync(topic.BlockId);
        var completedTopics = await _userProgressRepository.GetCompletedTopicIdsAsync(user.Id, topic.BlockId);

        var isLastTopic = allTopics.All(t => completedTopics.Contains(t.Id));
        if (isLastTopic)
        {
            var test = await _testRepository.GetByBlockIdAsync(topic.BlockId);
            if (test != null)
            {
                var keyboard = new InlineKeyboardMarkup(new[]
                {
                    InlineKeyboardButton.WithCallbackData("🧪 Пройти тест", $"test_{topic.BlockId}")
                });

                await _botClient.SendTextMessageAsync(
                    chatId,
                    "🎉 Вы прошли главу! Нажми, когда будешь готов!",
                    replyMarkup: keyboard,
                    cancellationToken: cancellationToken);
            }
        }
    }

    #endregion
}
