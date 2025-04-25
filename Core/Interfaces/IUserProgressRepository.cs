using Core.Entity;
using Core.Entity.Progress;

namespace Core.Interfaces;

public interface IUserProgressRepository
{
    Task SaveFinalTestResultAsync(long chatId, int finalTestId, int correctAnswersCount);
    // TODO: пересмотреть логику
    
    Task<TopicProgress?> GetTopicProgressAsync(int userId, int topicId);
    Task<FinalTestProgress?> GetFinalTestProgressAsync(int userId, int blockId);
    Task<BlockCompletionProgress?> GetBlockCompletionProgressAsync(int userId, int blockId);
   
    Task<List<int>> GetCompletedTopicIdsAsync(int userId, int blockId);
    Task MarkTopicCompletedAsync(int userId, int topicId);
}   