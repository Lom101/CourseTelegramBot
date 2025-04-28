using Core.Entity.Progress;

namespace Core.Model;

public class UserProgressDetails
{
    public int UserId { get; set; }

    // Прогресс по темам
    public List<TopicProgress> CompletedTopics { get; set; } = new List<TopicProgress>();

    // Прогресс по финальным тестам
    public List<FinalTestProgress> FinalTestProgresses { get; set; } = new List<FinalTestProgress>();

    // Прогресс по завершению блоков
    public List<BlockCompletionProgress> BlockCompletionProgresses { get; set; } = new List<BlockCompletionProgress>();
}