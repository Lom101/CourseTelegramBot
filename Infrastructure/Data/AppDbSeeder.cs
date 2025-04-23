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

        if (!context.Blocks.Any())
        {
            var course = new Block
            {
                Title = "Глава 1. Лидерство",
                Topics = new List<Topic>
                {
                    new Topic
                    {
                        Title = "Основы лидерства",
                        ContentItems = new List<ContentItem>
                        {
                            new BookContent
                            {
                                Title = "Книга Михаила Алистера 'Сверх продуктивность'",
                                FileUrl = "/upload/books/Сверх продуктивность, Михаил Алистер.pdf",
                                FileName = "Сверх продуктивность, Михаил Алистер.pdf"
                            },
                            // new BookContent
                            // {
                            //     Title = "Тайм-менеджмент",
                            //     Description = "Книга Брайана Трейси",
                            //     FileUrl = "/upload/books/Тайм-менеджмент, Брайан Трейси.pdf",
                            //     FileName = "Тайм-менеджмент, Брайан Трейси.pdf"
                            // },
                            // new ImageContent
                            // {
                            //     Title = "Метод АБВГД",
                            //     Description = "Изображение схемы",
                            //     ImageUrl = "/upload/pictures/метод АБВГД.jpg",
                            //     AltText = "метод АБВГД"
                            // }
                        }
                    },
                    // new Topic
                    // {
                    //     Title = "Практики командной работы",
                    //     Order = 2,
                    //     ContentItems = new List<ContentItem>
                    //     {
                    //         new BookContent
                    //         {
                    //             Title = "Пять пороков команды",
                    //             Description = "Книга Патрика Ленсиони",
                    //             FileUrl = "/upload/books/Пять_пороков_команды_Патрик Ленсиони.pdf",
                    //             FileName = "Пять_пороков_команды_Патрик Ленсиони.pdf"
                    //         },
                    //         new BookContent
                    //         {
                    //             Title = "Теория U",
                    //             Description = "Книга Отто Шармера",
                    //             FileUrl = "/upload/books/Теория U_Отто Шармер.pdf",
                    //             FileName = "Теория U_Отто Шармер.pdf"
                    //         }
                    //     }
                    //}
                }
            };

            context.Blocks.Add(course);
            await context.SaveChangesAsync();
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
                    IsAdmin = true,
                    PasswordHash = PasswordHasher.HashPassword("admin123")
                }
            };

            context.Users.AddRange(users);
        }

        await context.SaveChangesAsync();
    }
}
