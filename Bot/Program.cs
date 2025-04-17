using Bot.Helpers.ExceptionHandler;
using Bot.Helpers.ExceptionHandler.Intefaces;
using Bot.Service;
using Bot.Service.Interfaces;
using Core.Interfaces;
using Telegram.Bot;
using DotNetEnv;
using Infrastructure.Data;
using Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

Env.Load();
Env.TraversePath().Load();

var builder = Host.CreateApplicationBuilder(args);

// Получаем токен из .env
var botToken = Environment.GetEnvironmentVariable("TOKEN")
               ?? throw new ArgumentNullException("Не указан телеграм бот токен");
var connectionString = Environment.GetEnvironmentVariable("CONNECTION_STRING") 
                       ?? throw new ArgumentNullException("Не указан путь подключения к базе данных");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddSingleton<ITelegramBotClient>(new TelegramBotClient(botToken));

builder.Services.AddSingleton<IBackgroundExceptionHandler, BackgroundExceptionHandler>(); // сервис для логирования и отправки админу всех ошибок
builder.Services.AddHostedService<BotHostedService>(); // фоновый сервис для запуска бота

builder.Services.AddScoped<IUserBotService, UserBotService>(); // хэндлеры для обычных пользователей
builder.Services.AddScoped<IAdminBotService, AdminBotService>(); // админка

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ICourseRepository, CourseRepository>();
builder.Services.AddScoped<ITopicRepository, TopicRepository>();
builder.Services.AddScoped<IUserActivityRepository, UserActivityRepository>();
builder.Services.AddScoped<IUserProgressRepository, UserProgressRepository>();

var host = builder.Build();

using (var scope = host.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await AppDbSeeder.SeedAsync(db);
}

await host.RunAsync();