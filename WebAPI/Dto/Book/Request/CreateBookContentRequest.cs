namespace Backend.Dto.Book.Request;

public class CreateBookContentRequest
{
    public int TopicId { get; set; }
    public string Title { get; set; }
    public IFormFile File { get; set; }
}
