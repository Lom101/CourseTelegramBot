using Backend.Service.Interfaces;

namespace Backend.Service;

public class ImageFileService : IImageFileService
{
    private readonly string _imageFolderPath;
    private readonly string _imageUrlBase;

    public ImageFileService(IWebHostEnvironment env)
    {
        _imageFolderPath = Path.Combine(env.WebRootPath, "uploads", "images");
        _imageUrlBase = "/uploads/images";

        if (!Directory.Exists(_imageFolderPath))
            Directory.CreateDirectory(_imageFolderPath);
    }

    /// <summary>
    /// Сохраняет изображение и возвращает URL-адрес.
    /// </summary>
    public async Task<string> SaveImageAsync(IFormFile file)
    {
        if (file == null || file.Length == 0)
            throw new ArgumentException("Файл изображения пуст");

        var fileExtension = Path.GetExtension(file.FileName);
        var fileName = Guid.NewGuid().ToString() + fileExtension;
        var fullPath = Path.Combine(_imageFolderPath, fileName);

        await using (var stream = new FileStream(fullPath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        return $"{_imageUrlBase}/{fileName}";
    }

    /// <summary>
    /// Удаляет изображение по его URL (относительному пути).
    /// </summary>
    public bool DeleteImage(string imageUrl)
    {
        var fileName = Path.GetFileName(imageUrl);
        var fullPath = Path.Combine(_imageFolderPath, fileName);

        if (File.Exists(fullPath))
        {
            File.Delete(fullPath);
            return true;
        }

        return false;
    }
}