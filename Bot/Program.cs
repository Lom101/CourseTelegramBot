using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using DotNetEnv;

Env.Load();
Env.TraversePath().Load();
var botToken = Environment.GetEnvironmentVariable("TOKEN") ?? throw new ArgumentNullException("Не указан телеграм бот токен");

var botClient = new TelegramBotClient(botToken);
botClient.StartReceiving(
    async (bot, update, token) =>
    {
        if (update.Message?.Text != null)
        {
            Console.WriteLine($"Получено сообщение от {update.Message.Chat.Id}: {update.Message.Text}");
            await bot.SendTextMessageAsync(update.Message.Chat.Id, $"Ты написал: {update.Message.Text}", cancellationToken: token);
        }
    },
    (bot, exception, token) =>
    {
        Console.WriteLine($"Ошибка: {exception.Message}");
        return Task.CompletedTask;
    },
    new ReceiverOptions { AllowedUpdates = Array.Empty<UpdateType>() }
);

Console.WriteLine($"Бот запущен: @{(await botClient.GetMeAsync()).Username}");
Console.ReadLine();