using Bot.Service.Interfaces;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Bot.Service;

public class UserBotService : IUserBotService
{
    private readonly ITelegramBotClient _botClient;
    private readonly ILogger<UserBotService> _logger;
    
    public UserBotService(
        ITelegramBotClient botClient, 
        ILogger<UserBotService> logger)
    {
        _botClient = botClient;
        _logger = logger;
    }
    
    public async Task HandleMessageAsync(Message message, CancellationToken cancellationToken)
    {
        if (message.Text is null) return;
            
        var chatId = message.Chat.Id;
        var messageText = message.Text;
            
        _logger.LogInformation($"Получено сообщение: {messageText}");
        
        switch (messageText)
        {
            case "/start":
                await _botClient.SendTextMessageAsync(chatId, " Йоу, салам", cancellationToken: cancellationToken);
                break;
            default:
                await _botClient.SendTextMessageAsync(chatId, "🤔 Я не понимаю. Пожалуйста, выберите команду из меню.", cancellationToken: cancellationToken);
                break;
        }
    }

    public Task HandleCallbackQueryAsync(CallbackQuery callbackQuery, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}