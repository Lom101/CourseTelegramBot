using System.Reflection;
using System.Text;
using Backend.Middleware;
using Backend.Service;
using Backend.Service.Interfaces;
using Core.Interfaces;
using Infrastructure.Data;
using Infrastructure.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

if (File.Exists(".env.local"))
    DotNetEnv.Env.Load(".env.local");
else if (File.Exists(".env"))
    DotNetEnv.Env.Load();

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
builder.Services.AddScoped<IContentItemRepository, ContentItemRepository>();

// Добавление Swagger
builder.Services.AddEndpointsApiExplorer(); // Эксплорер для API
builder.Services.AddSwaggerGen(options => // Генерация Swagger документации
{
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    options.IncludeXmlComments(xmlPath);
    
    // Добавляем JWT-схему авторизации
    options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Введите токен в формате: Bearer {токен}"
    });

    // Применяем схему ко всем операциям
    options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins", builder => 
    {
        builder.AllowAnyOrigin() // Разрешаем любой источник
            .AllowAnyMethod() // Разрешаем любые HTTP методы (GET, POST, PUT и т.д.)
            .AllowAnyHeader(); // Разрешаем любые заголовки
    });
});

builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        var jwtSettings = builder.Configuration.GetSection("JwtSettings");
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings["Issuer"],
            ValidAudience = jwtSettings["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]))
        };
    });

builder.Services.AddAuthorization();

builder.Services.AddSingleton<JwtService>();

builder.Services.AddScoped<IImageFileService, ImageFileService>();
builder.Services.AddScoped<IBookFileService, BookFileService>();
builder.Services.AddScoped<IAudioFileService, AudioFileService>();
builder.Services.AddScoped<IWordFileService, WordFileService>();


var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

// Применяем CORS
app.UseCors("AllowAllOrigins");

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

app.UseStaticFiles();

app.MapControllers();
app.Run();