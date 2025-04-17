using Bot.Helpers.ExceptionHandler.Intefaces;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
namespace Bot.Helpers.ExceptionHandler;

public class BackgroundExceptionHandler : IBackgroundExceptionHandler
{
    private readonly IHostApplicationLifetime _applicationLifetime;
    private readonly ILogger<BackgroundExceptionHandler> _logger;
    private readonly ITelegramBotClient _botClient;
    private readonly long _adminChatId = 5833493765; // TODO: получать chatId в конструкторе через DI

    public BackgroundExceptionHandler(
        IHostApplicationLifetime applicationLifetime,
        ILogger<BackgroundExceptionHandler> logger,
        ITelegramBotClient botClient)
    {
        _applicationLifetime = applicationLifetime;
        _logger = logger;
        _botClient = botClient;
    }
    
    public async Task HandleExceptionAsync(Exception ex)
    {
        _logger.LogError(ex, "Произошла необработанная ошибка");

        // Отправляем ошибку в Telegram админу
        await _botClient.SendTextMessageAsync(_adminChatId, $"❗ Ошибка в боте: {ex.Message} {ex.StackTrace}");
    }
}