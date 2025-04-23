namespace Backend.Dto.Book;

public class GetBookContentResponse
{
    public int Id { get; set; }
    public string FileUrl { get; set; }
    public string FileName { get; set; }
    public string Title { get; set; }
    public int Order { get; set; }
}
