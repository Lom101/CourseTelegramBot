using Bot.Service.Interfaces;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

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
        if (message.Contact != null)
        {
            await HandlePhoneNumberAsync(message, cancellationToken);
            return;
        }
        
        if (message.Text is null) return;
            
        var chatId = message.Chat.Id;
        var messageText = message.Text;
            
        _logger.LogInformation($"Получено сообщение: {messageText}");
        
        
        switch (messageText)
        {
            case "/start":
                await SayHello(message, cancellationToken);
                await GetPhoneNumber(message, cancellationToken);
                break;
            default:
                await _botClient.SendTextMessageAsync(chatId, "🤔 Я не понимаю. Пожалуйста, выберите команду из меню.", cancellationToken: cancellationToken);
                break;
        }
    }

    private async Task GetPhoneNumber(Message message, CancellationToken cancellationToken)
    {
        var keyboard = new ReplyKeyboardMarkup(new[]
        {
            new KeyboardButton[]
            {
                KeyboardButton.WithRequestContact("📱 Отправить номер телефона")
            }
        })
        {
            ResizeKeyboard = true,
            OneTimeKeyboard = true
        };

        await _botClient.SendTextMessageAsync(
            chatId: message.Chat.Id,
            text: "Пожалуйста, отправьте свой номер телефона, нажав на кнопку ниже:",
            replyMarkup: keyboard
        );
    }

    public async Task SayHello(Message message, CancellationToken cancellationToken)
    {
        await _botClient.SendTextMessageAsync(message.Chat.Id, " Йоу, салам", cancellationToken: cancellationToken);
    }
    
    private async Task HandlePhoneNumberAsync(Message message, CancellationToken cancellationToken)
    {
        string phoneNumber = message.Contact.PhoneNumber;
        string firstName = message.Contact.FirstName;
        long userId = message.Contact.UserId ?? message.From.Id;

        _logger.LogInformation($"Получен контакт: {firstName}, {phoneNumber}, userId: {userId}");

        await _botClient.SendTextMessageAsync(
            chatId: message.Chat.Id,
            text: $"Спасибо! Ваш номер телефона: {phoneNumber}",
            cancellationToken: cancellationToken
        );

            // Здесь можно сохранить в базу данных, например:
            // await _userRepository.SavePhoneAsync(userId, phoneNumber);
    }


    public Task HandleCallbackQueryAsync(CallbackQuery callbackQuery, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}