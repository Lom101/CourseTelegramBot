﻿using Bot.Helpers.ExceptionHandler;
using Bot.Helpers.ExceptionHandler.Intefaces;
using Bot.Helpers.TestSession;
using Bot.Helpers.TestSession.Interface;
using Bot.Helpers.UserSession;
using Bot.Helpers.UserSession.Interface;
using Bot.Service;
using Bot.Service.Interfaces;
using Core.Interfaces;
using Telegram.Bot;
using Infrastructure.Data;
using Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

if (File.Exists(".env.local"))
    DotNetEnv.Env.Load(".env.local");
else if (File.Exists(".env"))
    DotNetEnv.Env.Load();

var builder = Host.CreateApplicationBuilder(args);

// Получаем токен из .env
var botToken = Environment.GetEnvironmentVariable("BotConfiguration__Token")
               ?? throw new ArgumentNullException("Не указан телеграм бот токен");
var connectionString = Environment.GetEnvironmentVariable("ConnectionStrings__DefaultConnection") 
                       ?? throw new ArgumentNullException("Не указан путь подключения к базе данных");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddSingleton<ITelegramBotClient>(new TelegramBotClient(botToken));

builder.Services.AddSingleton<IBackgroundExceptionHandler, BackgroundExceptionHandler>(); // сервис для логирования и отправки админу всех ошибок
builder.Services.AddHostedService<BotHostedService>(); // фоновый сервис для запуска бота
builder.Services.AddHostedService<UserNotificationHostedService>();

builder.Services.AddScoped<IUserBotService, UserBotService>(); // хэндлеры для обычных пользователей
builder.Services.AddScoped<IAdminBotService, AdminBotService>(); // админка

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IBlockRepository, BlockRepository>();
builder.Services.AddScoped<ITopicRepository, TopicRepository>();
builder.Services.AddScoped<IUserProgressRepository, UserProgressRepository>();
builder.Services.AddScoped<IFinalTestRepository, FinalTestRepository>();


builder.Services.AddSingleton<IUserSessionService, UserSessionService>();
builder.Services.AddSingleton<IFinalTestSessionService, FinalTestSessionService>();


var host = builder.Build();

using (var scope = host.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await AppDbSeeder.SeedAsync(db);
}

await host.RunAsync();