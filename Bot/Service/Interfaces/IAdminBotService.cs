using Telegram.Bot.Types;

namespace Bot.Service.Interfaces;

public interface IAdminBotService
{
    Task HandleMessageAsync(Message message, CancellationToken cancellationToken);
    Task HandleCallbackQueryAsync(CallbackQuery callbackQuery, CancellationToken cancellationToken);
}