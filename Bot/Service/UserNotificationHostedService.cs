using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Core.Interfaces;
using Telegram.Bot;

namespace Bot.Service
{
    public class UserNotificationHostedService : IHostedService, IDisposable
    {
        private readonly ITelegramBotClient _botClient;
        private readonly IUserRepository _userRepository;
        private Timer _timer;

        private readonly string[] _reminders = new[]
        {
            "⏰ Уже забываешь о курсе? Вернись и продолжи учиться, не подведи! 💪",
    
            "🚨 Ты что, остановился? Время продолжить курс и достичь цели! 🎯",
    
            "⚡️ Не откладывай на потом! Твой курс ждет, возвращайся к нему прямо сейчас! 📚",
    
            "👀 Ты не забыл про курс, да? Давай, продолжай, чтобы твои знания становились лучше! 💥"
        };


        public UserNotificationHostedService(ITelegramBotClient botClient, IUserRepository userRepository)
        {
            _botClient = botClient;
            _userRepository = userRepository;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            // Запускаем таймер, который будет опрашивать пользователей каждую минуту
            _timer = new Timer(SendInactivityNotifications, null, TimeSpan.Zero, TimeSpan.FromMinutes(1));
            return Task.CompletedTask;
        }

        private async void SendInactivityNotifications(object state)
        {
            var users = await _userRepository.GetAllAsync();
            var now = DateTime.UtcNow;

            foreach (var user in users)
            {
                // Если пользователь не был активен в течение последней минуты
                if (user.LastActivity.HasValue && now - user.LastActivity.Value > TimeSpan.FromMinutes(1))
                {
                    // Выбираем случайное напоминание из массива
                    var randomReminder = _reminders[new Random().Next(_reminders.Length)];

                    // Отправляем случайное напоминание пользователю
                    if (user.ChatId.HasValue)
                    {
                        await _botClient.SendTextMessageAsync(user.ChatId.Value, randomReminder);
                    }
                }
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            // Останавливаем таймер при остановке сервиса
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
