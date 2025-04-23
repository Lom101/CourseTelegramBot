namespace Core.Dto.Course.Response;

public class GetCourseResponse
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public int TopicsCount { get; set; }
    public List<GetTopicShortResponse> Topics { get; set; } = new();
}

public class GetTopicShortResponse
{
    public int Id { get; set; }
    public string Title { get; set; }
}