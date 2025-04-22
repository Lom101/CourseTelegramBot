using Core.Entity;
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
                Title = "Тим Лид",
                Description = "Образовательный курс для будущих тимлидов, включающий теорию, практику, видеоуроки и тесты.",
                Topics = new List<Topic>
                {
                    new Topic
                    {
                        Title = "Введение в лидерство",
                        Order = 1,
                        ContentItems = new List<ContentItem>
                        {
                            new TextContent
                            {
                                Title = "Роль тимлида",
                                Description = "Основные обязанности тимлида в IT-команде.",
                                Text = "Тимлид отвечает за техническое руководство командой, коммуникации с заказчиком и развитие членов команды."
                            },
                            new VideoContent
                            {
                                Title = "Кто такой тимлид?",
                                Description = "Интервью с опытным руководителем.",
                                VideoUrl = "https://example.com/videos/teamlead-role.mp4",
                                ThumbnailUrl = "https://example.com/thumbs/teamlead-role.jpg",
                                Duration = TimeSpan.FromMinutes(7)
                            }
                        }
                    },
                    new Topic
                    {
                        Title = "Эффективная коммуникация",
                        Order = 2,
                        ContentItems = new List<ContentItem>
                        {
                            new TextContent
                            {
                                Title = "Обратная связь",
                                Description = "Как давать конструктивную обратную связь.",
                                Text = "Обратная связь должна быть своевременной, конкретной и конструктивной, с акцентом на развитие."
                            },
                            new LinkContent
                            {
                                Title = "Статья на Хабре",
                                Description = "Практика общения внутри команды.",
                                Url = "https://habr.com/ru/company/example/blog/feedback-practice/"
                            }
                        }
                    },
                    new Topic
                    {
                        Title = "Управление задачами",
                        Order = 3,
                        ContentItems = new List<ContentItem>
                        {
                            new TextContent
                            {
                                Title = "Приоритезация",
                                Description = "Методы расстановки приоритетов.",
                                Text = "Популярные методы приоритезации: Eisenhower Matrix, MoSCoW, RICE и др."
                            },
                            new ImageContent
                            {
                                Title = "Матрица Эйзенхауэра",
                                Description = "Инструмент для принятия решений.",
                                ImageUrl = "https://example.com/images/eisenhower-matrix.png",
                                AltText = "Матрица Эйзенхауэра"
                            }
                        }
                    }
                }
            };
            context.Courses.Add(course);
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
