using Bot.Service.Interfaces;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Bot.Service;

public class AdminBotService : IAdminBotService
{
    private readonly ITelegramBotClient _botClient;
    private readonly ILogger<AdminBotService> _logger;
    
    public AdminBotService(
        ITelegramBotClient botClient, 
        ILogger<AdminBotService> logger)    
    {
        _botClient = botClient;  
        _logger = logger;
    }

    public async Task HandleMessageAsync(Message message, CancellationToken cancellationToken)
    {
        var chatId = message.Chat.Id;
        var messageText = message.Text;

        _logger.LogInformation($"[Admin] {chatId}: {messageText}");   
        switch (messageText)
        {
            case "/start":
                await _botClient.SendTextMessageAsync(chatId, "🚀 Привет, админ! Используй /help.", cancellationToken: cancellationToken);
                break;
            default:
                await _botClient.SendTextMessageAsync(chatId, "🚀 Команда админа не распознана", cancellationToken: cancellationToken);
                break;
        }
    }

    public Task HandleCallbackQueryAsync(CallbackQuery callbackQuery, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}