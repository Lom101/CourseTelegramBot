namespace Backend.Dto;

public class CreateImageContentRequest
{
    public int TopicId { get; set; }
    public IFormFile Image { get; set; }
}