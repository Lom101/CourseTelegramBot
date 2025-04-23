namespace Backend.Dto;

public class CreateAudioContentRequest
{
    public int TopicId { get; set; }
    public IFormFile AudioFile { get; set; }
    public string AudioTitle { get; set; }
}
