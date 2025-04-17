using Core.Entity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public static class AppDbSeeder
{
    public static async Task SeedAsync(AppDbContext context)
    {
        await context.Database.MigrateAsync();

        if (!context.Courses.Any())
        {
            var course = new Course
            {
                Title = "Основы C#",
                Description = "Пошаговый курс по основам языка C# с теорией и практикой.",
                Topics = new List<Topic>
                {
                    new Topic
                    {
                        Title = "Введение в C#",
                        Order = 1,
                        ContentItems = new List<ContentItem>
                        {
                            new TextContent
                            {
                                Title = "Что такое C#?",
                                Description = "История и назначение языка.",
                                Text = "C# — это современный, объектно-ориентированный язык программирования, разработанный Microsoft в рамках платформы .NET."
                            },
                            new VideoContent
                            {
                                Title = "Видеообзор языка",
                                Description = "Видеоурок по основам C#.",
                                VideoUrl = "https://example.com/videos/intro-csharp.mp4",
                                ThumbnailUrl = "https://example.com/thumbs/intro-csharp.jpg",
                                Duration = TimeSpan.FromMinutes(5)
                            }
                        }
                    },
                    new Topic
                    {
                        Title = "Переменные и типы данных",
                        Order = 2,
                        ContentItems = new List<ContentItem>
                        {
                            new TextContent
                            {
                                Title = "Типы данных",
                                Description = "Основные типы данных в C#.",
                                Text = "В C# есть несколько встроенных типов данных: int, double, string, bool и др."
                            },
                            new ImageContent
                            {
                                Title = "Схема типов",
                                Description = "Инфографика по типам данных.",
                                ImageUrl = "https://example.com/images/data-types.png",
                                AltText = "Типы данных в C#"
                            }
                        }
                    },
                    new Topic
                    {
                        Title = "Условные операторы",
                        Order = 3,
                        ContentItems = new List<ContentItem>
                        {
                            new TextContent
                            {
                                Title = "if-else",
                                Description = "Примеры использования условных операторов.",
                                Text = "Оператор `if` проверяет условие и выполняет блок кода, если условие истинно."
                            },
                            new LinkContent
                            {
                                Title = "Документация Microsoft",
                                Description = "Ссылка на официальную документацию.",
                                Url = "https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/statements/selection-statements"
                            }
                        }
                    }
                }
            };

            context.Courses.Add(course);
            await context.SaveChangesAsync();
        }
    }
}
