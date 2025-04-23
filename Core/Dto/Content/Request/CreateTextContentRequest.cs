namespace Core.Dto.Content.Request;

public class CreateTextContentRequest
{
    public int TopicId { get; set; }
    public string Text { get; set; }
}