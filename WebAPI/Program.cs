using Backend.Middleware;
using Core.Interfaces;
using DotNetEnv;
using Infrastructure.Data;
using Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;

if (File.Exists(".env.local"))
    DotNetEnv.Env.Load(".env.local");
else if (File.Exists(".env"))
    DotNetEnv.Env.Load();
Env.TraversePath().Load();

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
    .AddEnvironmentVariables();

builder.Services.AddControllers();

var connectionString = Environment.GetEnvironmentVariable("ConnectionStrings__DefaultConnection") 
                       ?? throw new ArgumentNullException("Не указан путь подключения к базе данных");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ICourseRepository, CourseRepository>();
builder.Services.AddScoped<ITopicRepository, TopicRepository>();
builder.Services.AddScoped<IUserActivityRepository, UserActivityRepository>();
builder.Services.AddScoped<IUserProgressRepository, UserProgressRepository>();

// Добавление Swagger
builder.Services.AddEndpointsApiExplorer(); // Эксплорер для API
builder.Services.AddSwaggerGen(); // Генерация Swagger документации

var app = builder.Build();
app.UseMiddleware<ExceptionHandlingMiddleware>();
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await AppDbSeeder.SeedAsync(db); // Сидирование данных
}

// Включаем использование Swagger в приложении
if (app.Environment.IsDevelopment())
{
    app.UseSwagger(); // Включаем Swagger
    app.UseSwaggerUI(); // UI для Swagger (автоматическая генерация документации)
}

app.MapControllers();
app.Run();