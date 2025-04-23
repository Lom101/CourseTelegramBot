namespace Backend.Dto;

public class CreateBookContentRequest
{
    public int TopicId { get; set; }
    public IFormFile File { get; set; } = null!;
}
