using Backend.Service.Interfaces;

namespace Backend.Service;

public class WordFileService : IWordFileService
{
    private readonly string _wordFolderPath;
    private readonly string _wordUrlBase;

    public WordFileService(IWebHostEnvironment env)
    {
        _wordFolderPath = Path.Combine(env.WebRootPath, "uploads", "docs");
        _wordUrlBase = "/uploads/docs";

        if (!Directory.Exists(_wordFolderPath))
            Directory.CreateDirectory(_wordFolderPath);
    }

    public async Task<string> SaveWordFileAsync(IFormFile file)
    {
        if (file == null || file.Length == 0)
            throw new ArgumentException("Файл Word пуст");

        var fileExtension = Path.GetExtension(file.FileName);
        var fileName = Guid.NewGuid().ToString() + fileExtension;
        var fullPath = Path.Combine(_wordFolderPath, fileName);

        await using (var stream = new FileStream(fullPath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        return $"{_wordUrlBase}/{fileName}";
    }

    public bool DeleteWordFile(string fileUrl)
    {
        var fileName = Path.GetFileName(fileUrl);
        var fullPath = Path.Combine(_wordFolderPath, fileName);

        if (File.Exists(fullPath))
        {
            File.Delete(fullPath);
            return true;
        }

        return false;
    }
}