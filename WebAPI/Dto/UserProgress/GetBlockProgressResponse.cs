namespace Backend.Dto.UserProgress;

public class GetBlockProgressResponse
{
    public int BlockId { get; set; }
    public string BlockTitle { get; set; }
    public bool IsBlockCompleted { get; set; }
    public List<GetTopicProgressResponse> TopicProgresses { get; set; }
    public GetFinalTestProgressResponse FinalTestProgress { get; set; }
}