using Core.Entity;
using Core.Entity.AnyContent;
using Infrastructure.Helpers;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public static class AppDbSeeder
{
    public static async Task SeedAsync(AppDbContext context)
    {
        await context.Database.EnsureDeletedAsync();
        //await context.Database.EnsureCreatedAsync();
        await context.Database.MigrateAsync();

       if (!context.Courses.Any())
{
    var course = new Course
    {
        Title = "Курс по Лидерству",
        Description = "Подборка контента для развития лидерских навыков.",
        Topics = new List<Topic>
        {
            new Topic
            {
                Title = "Основы лидерства",
                Order = 1,
                ContentItems = new List<ContentItem>
                {
                    new BookContent
                    {
                        Title = "Сверх продуктивность",
                        Description = "Книга Михаила Алистера",
                        FileUrl = "/files/books/Сверх продуктивность, Михаил Алистер.pdf",
                        FileName = "Сверх продуктивность, Михаил Алистер.pdf"
                    },
                    new BookContent
                    {
                        Title = "Тайм-менеджмент",
                        Description = "Книга Брайана Трейси",
                        FileUrl = "/files/books/Тайм-менеджмент, Брайан Трейси.pdf",
                        FileName = "Тайм-менеджмент, Брайан Трейси.pdf"
                    },
                    new TextContent
                    {
                        Title = "Лонгрид: АБВГДЕ",
                        Description = "Текст о лидерстве",
                        Text = File.ReadAllText("blocks/block1/texts/Лонгрид, ABCDE (АБВГД).txt")
                    },
                    new TextContent
                    {
                        Title = "Матрица Эйзенхауэра",
                        Description = "Текст о приоритезации",
                        Text = File.ReadAllText("blocks/block1/texts/Лонгрид. Матрица Эйзенхауэра.txt")
                    },
                    new ImageContent
                    {
                        Title = "Метод АБВГД",
                        Description = "Изображение схемы",
                        ImageUrl = "/files/pictures/метод АБВГД.jpg",
                        AltText = "метод АБВГД"
                    }
                }
            },
            new Topic
            {
                Title = "Практики командной работы",
                Order = 2,
                ContentItems = new List<ContentItem>
                {
                    new BookContent
                    {
                        Title = "Пять пороков команды",
                        Description = "Книга Патрика Ленсиони",
                        FileUrl = "/files/books/Пять_пороков_команды_Патрик Ленсиони.pdf",
                        FileName = "Пять_пороков_команды_Патрик Ленсиони.pdf"
                    },
                    new BookContent
                    {
                        Title = "Теория U",
                        Description = "Книга Отто Шармера",
                        FileUrl = "/files/books/Теория U_Отто Шармер.pdf",
                        FileName = "Теория U_Отто Шармер.pdf"
                    },
                    new TextContent
                    {
                        Title = "Кто такой лидер?",
                        Description = "Разбор понятий",
                        Text = File.ReadAllText("blocks/block2/texts/Лонгрид. Кто такой лидер.txt")
                    }
                }
            }
        }
    };

    context.Courses.Add(course);
    context.SaveChanges();
}

        if (!context.Users.Any())
        {
            var users = new List<User>
            {
                new User
                {
                    PhoneNumber = "+79534841869",
                    Email = "bulat@mail.com",
                    FullName = "Шакиров Булат Азатович",
                    RegistrationDate = DateTime.UtcNow,
                    LastActivity = DateTime.UtcNow,
                    IsBlocked = false,
                    IsAdmin = false
                },
                new User
                {
                    PhoneNumber = "+79997654321",
                    Email = "admin@mail.ru",
                    FullName = "Админов Админ Админович",
                    RegistrationDate = DateTime.UtcNow,
                    LastActivity = DateTime.UtcNow,
                    IsBlocked = false,
                    IsAdmin = true, // администратор
                    PasswordHash = PasswordHasher.HashPassword("admin123")
                }
            };

            context.Users.AddRange(users);
        }
        
        await context.SaveChangesAsync();
    }
}
