namespace Backend.Dto.Book;

public class GetBookContentResponse
{
    public int Id { get; set; }
    public string Description { get; set; }
    public string FileUrl { get; set; }
    public string FileName { get; set; }
    public string Title { get; set; }
    public string TopicTitle { get; set; }
}
