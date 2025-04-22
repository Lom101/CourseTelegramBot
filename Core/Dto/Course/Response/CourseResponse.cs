namespace Core.Dto.Course.Response;

public class CourseResponse
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public int TopicsCount { get; set; }
    public List<TopicShortResponse> Topics { get; set; } = new();
}

public class TopicShortResponse
{
    public int Id { get; set; }
    public string Title { get; set; }
}