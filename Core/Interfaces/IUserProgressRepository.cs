using Core.Entity;
using Core.Entity.Progress;

namespace Core.Interfaces;

public interface IUserProgressRepository
{
    Task<bool> IsTestCompletedAsync(long userId, int blockId);
    Task<TopicProgress?> GetTopicProgressAsync(int userId, int topicId);
    Task<BlockCompletionProgress?> GetBlockCompletionProgressAsync(int userId, int blockId);

    Task MarkBlockAsCompletedAsync(int userId, int blockId);
    Task MarkTopicCompletedAsync(int userId, int blockId);
    Task SaveFinalTestResultAsync(long chatId, int finalTestId, int correctAnswersCount);
    
    Task<List<int>> GetCompletedTopicIdsAsync(int userId, int blockId);
}   