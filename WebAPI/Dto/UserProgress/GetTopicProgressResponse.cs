namespace Backend.Dto.UserProgress;

public class GetTopicProgressResponse
{
    public int TopicId { get; set; }
    public string TopicTitle { get; set; }
    public bool IsTopicCompleted { get; set; }
}