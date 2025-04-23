namespace Core.Dto.Content.Request;

public class CreateImageContentDto
{
    public int TopicId { get; set; }
    public byte[] ImageBytes { get; set; }
    public string FileName { get; set; }
    public string ContentType { get; set; }
}