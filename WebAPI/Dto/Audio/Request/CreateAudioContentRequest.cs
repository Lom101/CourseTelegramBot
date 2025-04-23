namespace Backend.Dto;

public class CreateAudioContentRequest
{
    public int TopicId { get; set; }
    public string Title { get; set; }
    public IFormFile AudioFile { get; set; }
}