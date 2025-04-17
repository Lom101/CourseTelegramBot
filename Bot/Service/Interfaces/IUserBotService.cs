using Telegram.Bot.Types;

namespace Bot.Service.Interfaces;

public interface IUserBotService
{
    Task HandleMessageAsync(Message message, CancellationToken cancellationToken);
    Task HandleCallbackQueryAsync(CallbackQuery callbackQuery, CancellationToken cancellationToken);
}