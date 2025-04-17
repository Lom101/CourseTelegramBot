using Bot.Helpers.ExceptionHandler.Intefaces;
using Bot.Service.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Bot.Service;

/// <summary>
/// Сервис, отвечающий за работу Telegram-бота в фоновом режиме.
/// Реализует интерфейс <see cref="IHostedService"/>, что позволяет автоматически запускать и останавливать бота при старте/остановке приложения.
/// </summary>
public class BotHostedService : IHostedService
{
    private readonly ITelegramBotClient _botClient;
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly IBackgroundExceptionHandler _exceptionHandler;
    private readonly ILogger<BotHostedService> _logger;
    private CancellationTokenSource _cts;
    private static readonly long admin_chat_id = long.Parse(Environment.GetEnvironmentVariable("ADMIN_CHAT_ID") ?? throw new InvalidOperationException("admin_chat_id не указан в .env файле"));
        
    /// <summary>
    /// Конструктор BotHostedService.
    /// </summary>
    /// <param name="botClient">Клиент Telegram Bot API.</param>
    /// <param name="scopeFactory">Фабрика для создания областей видимости (Scopes) служб.</param>
    /// <param name="logger">Логгер для записи информации и ошибок.</param>
    public BotHostedService(
        ITelegramBotClient botClient, 
        IServiceScopeFactory scopeFactory, 
        IBackgroundExceptionHandler exceptionHandler,
        ILogger<BotHostedService> logger)
    {
        _botClient = botClient;
        _scopeFactory = scopeFactory;
        _exceptionHandler = exceptionHandler;
        _logger = logger;
    }

    /// <summary>
    /// Запускает Telegram-бота и начинает обработку входящих обновлений.
    /// </summary>
    /// <param name="cancellationToken">Токен отмены.</param>
    public Task StartAsync(CancellationToken cancellationToken)
    {
        _cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

        var receiverOptions = new ReceiverOptions
        {
            AllowedUpdates = Array.Empty<UpdateType>()
        };

        _botClient.StartReceiving(
            HandleUpdateAsync,
            HandlePollingErrorAsync,
            receiverOptions,
            _cts.Token
        );

        _logger.LogInformation("Бот запущен");
        return Task.CompletedTask;
    }
        
    /// <summary>
    /// Останавливает Telegram-бота и отменяет обработку обновлений.
    /// </summary>
    /// <param name="cancellationToken">Токен отмены.</param>
    public Task StopAsync(CancellationToken cancellationToken)
    {
        _cts.Cancel();
        _logger.LogInformation("Бот остановлен");
        return Task.CompletedTask;
    }
        
    private async Task HandleUpdateAsync(ITelegramBotClient bot, Update update, CancellationToken token)
    {
        using var scope = _scopeFactory.CreateScope();
        var userBotService = scope.ServiceProvider.GetRequiredService<IUserBotService>();
        var adminBotService = scope.ServiceProvider.GetRequiredService<IAdminBotService>();
            
        try
        {
            switch (update.Type)
            {
                case UpdateType.Message when update.Message is { } message:
                    if (message.Chat.Id == admin_chat_id)
                    {
                        await adminBotService.HandleMessageAsync(message, token);
                    }
                    else
                    {
                        await userBotService.HandleMessageAsync(message, token);
                    }
                    break;
                case UpdateType.CallbackQuery when update.CallbackQuery is { } callbackQuery:
                    if (callbackQuery.Message.Chat.Id == admin_chat_id)
                    {
                        await adminBotService.HandleCallbackQueryAsync(callbackQuery, token);
                    }
                    else
                    {
                        await userBotService.HandleCallbackQueryAsync(callbackQuery, token);
                    }
                    break;
            }
        }
        catch (Exception ex)
        {
            await _exceptionHandler.HandleExceptionAsync(ex);
        }
    }

    private Task HandlePollingErrorAsync(ITelegramBotClient bot, Exception exception, CancellationToken token)
    {
        _exceptionHandler.HandleExceptionAsync(exception);
        _logger.LogError(exception, "Ошибка при получении обновлений");
        return Task.CompletedTask;
    }
}