using Bot.Helpers.ExceptionHandler;
using Bot.Helpers.ExceptionHandler.Intefaces;
using Bot.Service;
using Bot.Service.Interfaces;
using Telegram.Bot;
using DotNetEnv;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

Env.Load();
Env.TraversePath().Load();

var builder = Host.CreateApplicationBuilder(args);

// Получаем токен из .env
var botToken = Environment.GetEnvironmentVariable("TOKEN")
               ?? throw new ArgumentNullException("Не указан телеграм бот токен");

builder.Services.AddSingleton<ITelegramBotClient>(new TelegramBotClient(botToken));

builder.Services.AddSingleton<IBackgroundExceptionHandler, BackgroundExceptionHandler>(); // сервис для логирования и отправки админу всех ошибок
builder.Services.AddHostedService<BotHostedService>(); // фоновый сервис для запуска бота

builder.Services.AddScoped<IUserBotService, UserBotService>(); // хэндлеры для обычных пользователей
builder.Services.AddScoped<IAdminBotService, AdminBotService>(); // админка



var host = builder.Build();
await host.RunAsync();