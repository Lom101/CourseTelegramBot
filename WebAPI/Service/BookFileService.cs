using Backend.Service.Interfaces;

namespace Backend.Service;

public class BookFileService : IBookFileService
{
    private readonly string _bookFolderPath;
    private readonly string _bookUrlBase;

    public BookFileService(IWebHostEnvironment env)
    {
        _bookFolderPath = Path.Combine(env.WebRootPath, "uploads", "books");
        _bookUrlBase = "/uploads/books";

        if (!Directory.Exists(_bookFolderPath))
            Directory.CreateDirectory(_bookFolderPath);
    }

    public async Task<string> SaveBookAsync(IFormFile file)
    {
        if (file == null || file.Length == 0)
            throw new ArgumentException("Файл книги пуст");

        var fileExtension = Path.GetExtension(file.FileName);
        var fileName = Guid.NewGuid() + fileExtension;
        var fullPath = Path.Combine(_bookFolderPath, fileName);

        await using var stream = new FileStream(fullPath, FileMode.Create);
        await file.CopyToAsync(stream);

        return $"{_bookUrlBase}/{fileName}";
    }

    public bool DeleteBook(string bookUrl)
    {
        var fileName = Path.GetFileName(bookUrl);
        var fullPath = Path.Combine(_bookFolderPath, fileName);

        if (File.Exists(fullPath))
        {
            File.Delete(fullPath);
            return true;
        }

        return false;
    }
}