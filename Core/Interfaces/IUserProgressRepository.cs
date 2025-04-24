using Core.Entity;

namespace Core.Interfaces;

public interface IUserProgressRepository
{
    Task<TopicProgress?> GetTopicProgressAsync(int userId, int topicId);
    Task<FinalTestProgress?> GetFinalTestProgressAsync(int userId, int blockId);
    Task<BlockCompletionProgress?> GetBlockCompletionProgressAsync(int userId, int blockId);

    Task SaveTopicProgressAsync(TopicProgress progress);
    Task SaveFinalTestProgressAsync(FinalTestProgress progress);
    Task SaveCourseCompletionProgressAsync(BlockCompletionProgress progress);
    
    Task<List<int>> GetCompletedTopicIdsAsync(int userId, int blockId);
    Task MarkTopicCompletedAsync(int userId, int topicId);

}   