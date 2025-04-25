using Core.Entity;
using Core.Entity.AnyContent;
using Core.Entity.Progress;
using Core.Entity.Test;
using Infrastructure.Helpers;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public static class AppDbSeeder
    {
        public static async Task SeedAsync(AppDbContext context)
        {
            await context.Database.EnsureDeletedAsync();
            await context.Database.MigrateAsync();

            // Проверяем, если тесты и блоки не созданы
            if (!context.FinalTests.Any())
            {
                var test = new FinalTest
                {
                    Title = "Тест первой главе"
                };

                var question1 = new TestQuestion
                {
                    QuestionText = "Какой из следующих подходов наиболее эффективен для определения приоритетов задач?",
                    Options = new List<TestOption>
                    {
                        new TestOption { OptionText = "Метод ABCDE" },
                        new TestOption { OptionText = "Метод \"Сделай это позже\"" },
                        new TestOption { OptionText = "Метод \"Случайного выбора\"" }
                    },
                    CorrectIndex = 0 // Индекс правильного ответа (0 — это  "Метод ABCDE")
                };

                var question2 = new TestQuestion
                {
                    QuestionText = "Какой из следующих инструментов лучше всего подходит для визуализации задач и их сроков?",
                    Options = new List<TestOption>
                    {
                        new TestOption { OptionText = "Календарь" },
                        new TestOption { OptionText = "Доска задач (Kanban)" },
                        new TestOption { OptionText = "Тетрадь" }
                    },
                    CorrectIndex = 1 // Правильный ответ - "Доска задач (Kanban)"
                };

                test.Questions.Add(question1);
                test.Questions.Add(question2);

                context.FinalTests.Add(test);
                context.SaveChanges();
            }
        
            
            if (!context.Blocks.Any())
            {
                
                var block1 = new Block
                {
                    Title = "Блок 1",
                    FinalTestId = 1,
                    Topics = new List<Topic>
                    {
                        new Topic
                        {
                            Title = "Тайм-менеджмент тим лида",
                            ContentItems = new List<ContentItem>
                            {
                                new BookContent
                                {
                                    Title = "Сверх продуктивность",
                                    FileUrl = "/uploads/books/Сверх продуктивность Михаил Алистер.pdf",
                                    FileName = "Сверх продуктивность, Михаил Алистер.pdf"
                                },
                                new BookContent
                                {
                                    Title = "Тайм-менеджмент",
                                    FileUrl = "/uploads/books/Тайм-менеджмент, Брайан Трейси.pdf",
                                    FileName = "Тайм-менеджмент, Брайан Трейси.pdf"
                                },
                                new WordFileContent
                                {
                                    FileName = "1 Лонгрид. тайм-менеджмент тим лида.docx",
                                    FileUrl = "/uploads/docs/1 Лонгрид. тайм-менеджмент тим лида.docx",
                                    Title = "Тайм-менеджмент тим лида"
                                }
                            }
                        },
                        new Topic
                        {
                            Title = "Матрица Эйзенхауэра",
                            ContentItems = new List<ContentItem>
                            {
                                new WordFileContent
                                {
                                    FileName = "2 Лонгрид. Матрица Эйзенхауэра.docx",
                                    FileUrl = "/uploads/docs/2 Лонгрид. Матрица Эйзенхауэра.docx",
                                    Title = "Матрица Эйзенхауэра"
                                },
                                new AudioContent
                                {
                                    Title = "Аудио",
                                    AudioUrl = "/uploads/audios/Аудио.m4a"
                                },
                                new ImageContent
                                {
                                    Title = "Метод Матрица Эйзенхауэра",
                                    ImageUrl = "/uploads/pictures/метод Матрица Эйзенхуэра.jpg",
                                    AltText = "Временно недоступно"
                                }
                            }
                        },
                        new Topic
                        {
                            Title = "ABCDE (АБВГД)",
                            ContentItems = new List<ContentItem>
                            {
                                new WordFileContent
                                {
                                    FileName = "3 Лонгрид. ABCDE (АБВГД).docx",
                                    FileUrl = "/uploads/docs/3 Лонгрид. ABCDE (АБВГД).docx",
                                    Title = "ABCDE (АБВГД)"
                                },
                                new ImageContent{
                                    Title = "Метод АБВГД",
                                    ImageUrl = "/uploads/pictures/Метод АБВГД.jpg",
                                    AltText = "Временно недоступно"
                                }
                            }
                        }
                    }
                };

                context.Blocks.Add(block1);
                await context.SaveChangesAsync();
            }

            if (!context.Blocks.Any(c => c.Title == "Блок 2"))
            {
                var course2 = new Block()
                {
                    Title = "Блок 2",
                    FinalTestId = 1,
                    Topics = new List<Topic>
                    {
                        new Topic
                        {
                            Title = "Кто такой лидер",
                            ContentItems = new List<ContentItem>
                            {
                                new BookContent
                                {
                                    Title = "Пять_пороков_команды",
                                    FileUrl = "/uploads/books/Пять_пороков_команды_Патрик Ленсиони.pdf",
                                    FileName = "Пять_пороков_команды_Патрик Ленсиони.pdf"
                                },
                                new BookContent
                                {
                                    Title = "Теория U",
                                    FileUrl = "/uploads/books/Теория U_Отто Шармер.pdf",
                                    FileName = "Теория U_Отто Шармер.pdf"
                                },
                                new WordFileContent
                                {
                                    FileName = "1 Лонгрид. Кто такой лидер.docx",
                                    FileUrl = "/uploads/docs/1 Лонгрид. Кто такой лидер.docx",
                                    Title = "Кто такой лидер"
                                }
                            }
                        },
                        new Topic
                        {
                            Title = "Стили лидерства",
                            ContentItems = new List<ContentItem>
                            {
                                new WordFileContent
                                {
                                    FileName = "2 Лонгрид. Стили лидерства.docx",
                                    FileUrl = "/uploads/docs/2 Лонгрид. Стили лидерства.docx",
                                    Title = "Стили лидерства"
                                },
                                new ImageContent
                                {
                                    Title = "картинка к лонгриду 2 (1)",
                                    ImageUrl = "/uploads/pictures/\"картинка к лонгриду 2 (1).jpg",
                                    AltText = "Временно недоступно"
                                },
                                new ImageContent
                                {
                                    Title = "картинка к лонгриду 2",
                                    ImageUrl = "/uploads/pictures/картинка к лонгриду 2.jpg",
                                    AltText = "Временно недоступно"
                                }
                            }
                        }
                    }
                };

                context.Blocks.Add(course2);
                await context.SaveChangesAsync();
            }
            if (!context.Blocks.Any(c => c.Title == "Блок 3"))
            {
                var course3 = new Block
                {
                    Title = "Блок 3",
                    FinalTestId = 1,
                    Topics = new List<Topic>
                    {
                        new Topic
                        {
                            Title = "Постановка задач (модели)",
                            ContentItems = new List<ContentItem>
                            {
                                new WordFileContent
                                {
                                    FileName = "Лонгрид 1. Постановка задач (модели).docx",
                                    FileUrl = "/uploads/docs/Лонгрид 1. Постановка задач (модели).docx",
                                    Title = "Постановка задач (модели)"
                                },
                                new ImageContent
                                {
                                    Title = "Модель HD-RW-RM",
                                    ImageUrl = "/uploads/pictures/Модель HD-RW-RM.jpg",
                                    AltText = "Временно недоступно"
                                },
                                new ImageContent
                                {
                                    Title = "Модель SMART",
                                    ImageUrl = "/uploads/pictures/Модель SMART.jpg",
                                    AltText = "Временно недоступно"
                                },
                                new ImageContent
                                {
                                    Title = "Модель TOTE",
                                    ImageUrl = "/uploads/pictures/Модель TOTE.jpg",
                                    AltText = "Временно недоступно"
                                }
                            }
                        },
                        new Topic
                        {
                            Title = "Делегирование",
                            ContentItems = new List<ContentItem>
                            {
                                new WordFileContent
                                {
                                    FileName = "Лонгрид 2. Делегирование.docx",
                                    FileUrl = "/uploads/docs/Лонгрид 2. Делегирование.docx",
                                    Title = "Делегирование"
                                },
                                new BookContent
                                {
                                    Title = "Делегирование и управление",
                                    FileUrl = "/uploads/books/Делегирование и управление_Трейси Б.pdf",
                                    FileName = "Делегирование и управление_Трейси Б.pdf"
                                },
                                new BookContent
                                {
                                    Title = "Делегирование",
                                    FileUrl = "/uploads/books/Делегирование. Фридман.pdf",
                                    FileName = "Делегирование. Фридман.pdf"
                                },
                                new AudioContent
                                {
                                    Title = "Делегирование",
                                    AudioUrl = "/uploads/audios/Аудио.m4a"
                                }
                            }
                        }
                    }
                };

                context.Blocks.Add(course3);
                await context.SaveChangesAsync();
            }
            if (!context.Blocks.Any(c => c.Title == "Блок 4"))
            {
                var course4 = new Block()
                {
                    Title = "Блок 4",
                    FinalTestId = 1,
                    Topics = new List<Topic>
                    {
                        new Topic
                        {
                            Title = "Культура совместной работы",
                            ContentItems = new List<ContentItem>
                            {
                                new WordFileContent
                                {
                                    FileName = "Лонгрид 1. Культура совместной работы.docx",
                                    FileUrl = "/uploads/docs/Лонгрид 1. Культура совместной работы.docx",
                                    Title = "Культура совместной работы"
                                },
                                new AudioContent
                                {
                                    Title = "Культура совместной работы. правила тимлида",
                                    AudioUrl = "" // TODO:
                                }
                            }
                        },
                        new Topic
                        {
                            Title = "Роли в команде",
                            ContentItems = new List<ContentItem>
                            {
                                new WordFileContent
                                {
                                    FileName = "Лонгрид 2. Роли в команде.docx",
                                    FileUrl = "/uploads/docs/Лонгрид 2. Роли в команде.docx",
                                    Title = "Роли в команде"
                                },
                                new BookContent
                                {
                                    Title = "Пять_пороков_команды_Притчи_о_лидерстве",
                                    FileUrl = "/uploads/books/Пять_пороков_команды_Притчи_о_лидерстве.pdf",
                                    FileName = "Пять_пороков_команды_Притчи_о_лидерстве.pdf"
                                }
                            }
                        }
                    }
                };

                context.Blocks.Add(course4);
                await context.SaveChangesAsync();
            }

            if (!context.Users.Any())
            {
                var users = new List<User>
                {
                    new User
                    {
                        PhoneNumber = "+79534841869",
                        ChatId = 740773865,
                        Email = "bulat@mail.com",
                        FullName = "Шакиров Булат Азатович",
                        RegistrationDate = DateTime.UtcNow,
                        LastActivity = DateTime.UtcNow,
                        IsBlocked = false,
                        IsAdmin = false
                    },
                    new User
                    {
                        PhoneNumber = "+79033883307",
                        Email = "123@mail.ru",
                        FullName = "Куколка",
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
                await context.SaveChangesAsync();
            }
            
            if (!context.TopicProgresses.Any())
            {
                var topicProgress = new List<TopicProgress>
                {
                    new TopicProgress { UserId = 1, BlockId = 1, TopicId = 1, IsCompleted = true },
                    new TopicProgress { UserId = 1, BlockId = 1 ,TopicId = 2, IsCompleted = true },
                };
                context.TopicProgresses.AddRange(topicProgress);
                context.SaveChanges();
            }

            // if (!context.FinalTestProgresses.Any())
            // {
            //     var finalTestProgress = new List<FinalTestProgress>
            //     {
            //         new FinalTestProgress { UserId = 1, BlockId = 1, IsPassed = true}
            //     };
            //     context.FinalTestProgresses.AddRange(finalTestProgress);
            //     context.SaveChanges();
            // }

            // if (!context.BlockCompletionProgresses.Any())
            // {
            //     var courseCompletionProgress = new List<BlockCompletionProgress>
            //     {
            //         new BlockCompletionProgress { UserId = 1, BlockId = 1, IsBlockCompleted = true },
            //     };
            //     context.BlockCompletionProgresses.AddRange(courseCompletionProgress);
            //     context.SaveChanges();
            // }
        }
    }
}