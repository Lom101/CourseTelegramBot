using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Core.Interfaces;
using Telegram.Bot;
using Telegram.Bot.Exceptions;

namespace Bot.Service
{
    public class UserNotificationHostedService : IHostedService, IDisposable
    {
        private readonly ITelegramBotClient _botClient;
        private readonly IUserRepository _userRepository;
        private Timer _timer;

        // Настройки, которые можно легко настроить в начале класса
        private readonly TimeSpan _notificationInterval = TimeSpan.FromMinutes(10); // Частота проверки активности пользователей (10 минут)
        private readonly TimeSpan _inactivityThreshold = TimeSpan.FromMinutes(50); // Время бездействия (50 минут)

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

            // Выводим параметры в консоль при старте сервиса
            Console.WriteLine($"[UserNotificationService] Напоминания будут отправляться каждые {_notificationInterval.TotalMinutes} минут.");
            Console.WriteLine($"[UserNotificationService] Напоминание будет отправляться, если пользователь молчит больше чем {_inactivityThreshold.TotalMinutes} минут.");
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            // Запускаем таймер с указанной частотой
            _timer = new Timer(SendInactivityNotifications, null, TimeSpan.Zero, _notificationInterval);
            return Task.CompletedTask;
        }

        private async void SendInactivityNotifications(object state)
        {
            var users = await _userRepository.GetAllAsync();
            var now = DateTime.UtcNow;

            foreach (var user in users)
            {
                // Если пользователь заблокирован или нет chatId, пропускаем его
                if (user.IsBlocked || user.ChatId == null)
                {
                    continue;
                }

                // Если пользователь не был активен в течение заданного времени
                if (user.LastActivity.HasValue && now - user.LastActivity.Value > _inactivityThreshold)
                {
                    // Выбираем случайное напоминание из массива
                    var randomReminder = _reminders[new Random().Next(_reminders.Length)];

                    try
                    {
                        // Отправляем случайное напоминание пользователю
                        if (user.ChatId.HasValue)
                        {
                            await _botClient.SendTextMessageAsync(user.ChatId.Value, randomReminder);
                        }
                    }
                    catch (ApiRequestException ex)
                    {
                        // Проверяем, заблокировал ли пользователь бота
                        if (ex.ErrorCode == 403)
                        {
                            // Логируем, что пользователь заблокировал бота
                            Console.WriteLine($"Пользователь с chatId {user.ChatId.Value} заблокировал бота.");
                        }
                        else
                        {
                            // Логируем другие ошибки
                            Console.WriteLine($"Ошибка при отправке сообщения пользователю с chatId {user.ChatId.Value}: {ex.Message}");
                        }
                    }
                    catch (Exception ex)
                    {
                        // Логируем остальные исключения
                        Console.WriteLine($"Неизвестная ошибка при отправке сообщения пользователю с chatId {user.ChatId.Value}: {ex.Message}");
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
