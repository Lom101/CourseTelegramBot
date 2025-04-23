namespace Backend.Service.Interfaces;

public interface IImageFileService
{
    /// <summary>
    /// Сохраняет изображение и возвращает URL-адрес.
    /// </summary>
    Task<string> SaveImageAsync(IFormFile file);

    /// <summary>
    /// Удаляет изображение по его URL (относительному пути).
    /// </summary>
    bool DeleteImage(string imageUrl);
}