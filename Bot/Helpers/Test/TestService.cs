using Bot.Helpers.Test.Interface;
using Bot.Service.Interfaces;
using Core.Entity.Test;
using Core.Interfaces;

namespace Bot.Helpers.Test;

public class TestService : ITestService
{
    private readonly ITestRepository _testRepository;
    private readonly IUserTestRepository _userTestRepository;

    public TestService(ITestRepository testRepository, IUserTestRepository userTestRepository)
    {
        _testRepository = testRepository;
        _userTestRepository = userTestRepository;
    }
    
    public async Task<Core.Entity.Test.Test> GetTestByTopicIdAsync(int topicId)
    {
        return await _testRepository.GetByTopicIdAsync(topicId);
    }


    public async Task StartTestAsync(long chatId, int testId)
    {
        var existingTest = await _userTestRepository.GetByChatIdAndTestIdAsync(chatId, testId);

        if (existingTest != null && existingTest.IsCompleted)
        {
            // Пользователь уже прошёл этот тест
            throw new InvalidOperationException("Вы уже проходили этот тест.");
        }

        if (existingTest == null)
        {
            var userTest = new UserTest
            {
                ChatId = chatId,
                TestId = testId,
                CurrentQuestionIndex = 0,
                IsCompleted = false
            };

            await _userTestRepository.AddAsync(userTest);
        }
        // Иначе пользователь начал, но не завершил — пусть продолжает с текущего места
    }


    // Получить тест пользователя
    public async Task<UserTest?> GetUserTestAsync(long chatId)
    {
        return await _userTestRepository.GetByChatIdAsync(chatId);
    }

    // Сохранить ответ пользователя
    public async Task SaveUserAnswerAsync(long chatId, int questionId, int selectedIndex, bool isCorrect)
    {
        var userTest = await _userTestRepository.GetByChatIdAsync(chatId);

        if (userTest == null) return;

        var answer = new UserTestAnswer
        {
            UserTestId = userTest.Id,
            QuestionId = questionId,
            SelectedIndex = selectedIndex,
            IsCorrect = isCorrect
        };
    
        await _userTestRepository.SaveAnswerAsync(answer);
        
        // Обновляем статус завершенности теста
        if (userTest.CurrentQuestionIndex + 1 >= userTest.Test.Questions.Count)
        {
            userTest.IsCompleted = true;
        }
        else
        {
            userTest.CurrentQuestionIndex++;
        }

        await _userTestRepository.UpdateAsync(userTest);
    }
    public async Task<bool> HasNextQuestionAsync(long chatId)
    {
        var userTest = await _userTestRepository.GetByChatIdAsync(chatId);
        if (userTest == null)
        {
            return false;  // Если сессия не найдена
        }

        if (userTest.Test == null)
        {
            return false;
        }

        // Логируем количество вопросов
        var questions = userTest.Test.Questions.OrderBy(q => q.Id).ToList();

        // Проверяем, есть ли следующий вопрос
        bool hasNext = userTest.CurrentQuestionIndex <= questions.Count - 1;

        if (userTest.IsCompleted == true)
        {
            hasNext = false;
        }

        return hasNext;
    }

    public async Task<TestQuestion> GetNextQuestionAsync(long chatId)
    {
        var userTest = await _userTestRepository.GetByChatIdAsync(chatId);
        if (userTest == null)
        {
            throw new InvalidOperationException("Тест пользователя не найден.");
        }

        if (userTest.Test == null)
        {
            throw new InvalidOperationException("Тест не найден.");
        }

        // Логируем количество вопросов
        var questions = userTest.Test.Questions.OrderBy(q => q.Id).ToList();

        // Проверяем, есть ли следующий вопрос
        if (userTest.CurrentQuestionIndex >= questions.Count)
        {
            throw new InvalidOperationException("Тест завершен или не найден следующий вопрос.");
        }

        // Логируем следующий вопрос
        var nextQuestion = questions[userTest.CurrentQuestionIndex];

        // Возвращаем следующий вопрос
        return nextQuestion;
    }
}