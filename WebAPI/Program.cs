using Core.Interfaces;
using Infrastructure.Data;
using Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
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