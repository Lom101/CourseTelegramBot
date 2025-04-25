namespace Core.Entity.Progress;

public class TopicProgress : UserProgress
{
    public int TopicId { get; set; }
    public bool IsCompleted { get; set; }
}