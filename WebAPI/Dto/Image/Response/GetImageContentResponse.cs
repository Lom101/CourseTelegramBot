namespace Backend.Dto.Image.Response;

public class GetImageContentResponse
{
    public int Id { get; set; }
    public string FileUrl { get; set; }
    public string AltText { get; set; }
    public string Title { get; set; }
}