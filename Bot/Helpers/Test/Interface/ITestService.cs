using Core.Entity.Test;

namespace Bot.Helpers.Test.Interface;

public interface ITestService
{
    Task<Core.Entity.Test.Test> GetTestByTopicIdAsync(int topicId);
    
    Task StartTestAsync(long chatId, int testId);
    Task<UserTest?> GetUserTestAsync(long chatId);
    Task SaveUserAnswerAsync(long chatId, int questionId, int selectedIndex, bool isCorrect);
    Task<bool> HasNextQuestionAsync(long chatId);
    Task<TestQuestion> GetNextQuestionAsync(long chatId);
}
