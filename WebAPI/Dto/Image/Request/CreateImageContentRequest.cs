namespace Backend.Dto.Image.Request;

public class CreateImageContentRequest
{
    public int TopicId { get; set; }
    public string Title { get; set; }
    public IFormFile Image { get; set; }
    public string AltText { get; set; }
}