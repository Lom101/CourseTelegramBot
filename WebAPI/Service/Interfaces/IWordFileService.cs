namespace Backend.Service.Interfaces;

public interface IWordFileService
{
    Task<string> SaveWordFileAsync(IFormFile file);
    bool DeleteWordFile(string fileUrl);
}