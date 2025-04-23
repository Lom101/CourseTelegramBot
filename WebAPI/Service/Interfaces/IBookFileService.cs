namespace Backend.Service.Interfaces;

public interface IBookFileService
{
    Task<string> SaveBookAsync(IFormFile file);
    bool DeleteBook(string bookUrl);
}
