using Backend.Service.Interfaces;

namespace Backend.Service;

public class AudioFileService : IAudioFileService
{
    private readonly string _audioFolderPath;
    private readonly string _audioUrlBase;

    public AudioFileService(IWebHostEnvironment env)
    {
        _audioFolderPath = Path.Combine(env.WebRootPath, "uploads", "audio");
        _audioUrlBase = "/uploads/audio";

        if (!Directory.Exists(_audioFolderPath))
            Directory.CreateDirectory(_audioFolderPath);
    }

    /// <summary>
    /// Сохраняет аудиофайл и возвращает его URL.
    /// </summary>
    public async Task<string> SaveAudioAsync(IFormFile file)
    {
        if (file == null || file.Length == 0)
            throw new ArgumentException("Файл аудио пуст");

        var fileExtension = Path.GetExtension(file.FileName);
        var fileName = Guid.NewGuid().ToString() + fileExtension;
        var fullPath = Path.Combine(_audioFolderPath, fileName);

        await using (var stream = new FileStream(fullPath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        return $"{_audioUrlBase}/{fileName}";
    }

    /// <summary>
    /// Удаляет аудиофайл по его URL (относительному пути).
    /// </summary>
    public bool DeleteAudio(string audioUrl)
    {
        var fileName = Path.GetFileName(audioUrl);
        var fullPath = Path.Combine(_audioFolderPath, fileName);

        if (File.Exists(fullPath))
        {
            File.Delete(fullPath);
            return true;
        }

        return false;
    }
}