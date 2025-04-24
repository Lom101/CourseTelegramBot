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
        var userTest = new UserTest
        {
            ChatId = chatId,
            TestId = testId,
            CurrentQuestionIndex = 0, // Начинаем с первого вопроса
            IsCompleted = false
        };

        await _userTestRepository.AddAsync(userTest);
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

        var test = await _testRepository.GetByIdAsync(userTest.TestId); // получаем объект
        
        // Обновляем статус завершенности теста
        if (userTest.CurrentQuestionIndex + 1 >= test.Questions.Count)
        {
            userTest.IsCompleted = true;
        }
        else
        {
            userTest.CurrentQuestionIndex++;
        }

        await _userTestRepository.UpdateAsync(userTest);
    }

    // Проверка, есть ли следующий вопрос
    public async Task<bool> HasNextQuestionAsync(long chatId)
    {
        var userTest = await _userTestRepository.GetByChatIdAsync(chatId);
        var test = await _testRepository.GetByIdAsync(userTest.TestId); // получаем объект
        return userTest != null && userTest.CurrentQuestionIndex < test.Questions.Count - 1;
    }

    // Получить следующий вопрос
    public async Task<TestQuestion> GetNextQuestionAsync(long chatId)
    {
        var userTest = await _userTestRepository.GetByChatIdAsync(chatId);
        var test = await _testRepository.GetByIdAsync(userTest.TestId); // получаем объект

        if (userTest == null || userTest.CurrentQuestionIndex >= test.Questions.Count)
        {
            throw new InvalidOperationException("Тест завершен или не найден.");
        }

        var questions = test.Questions.ToList();
        return questions[userTest.CurrentQuestionIndex + 1];
    }
}