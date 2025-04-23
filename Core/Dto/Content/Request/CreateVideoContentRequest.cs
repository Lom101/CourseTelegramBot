namespace Core.Dto.Content.Request;

public class CreateVideoContentRequest
{
    public int TopicId { get; set; }
    public string VideoUrl { get; set; }
}