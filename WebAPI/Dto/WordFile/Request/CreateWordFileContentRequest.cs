public class CreateWordFileContentRequest
{
    public int TopicId { get; set; }
    public string Title { get; set; }
    public IFormFile File { get; set; }
}