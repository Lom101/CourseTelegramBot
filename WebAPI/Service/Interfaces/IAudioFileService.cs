namespace Backend.Service.Interfaces;

public interface IAudioFileService
{
    Task<string> SaveAudioAsync(IFormFile audioFile);
    bool DeleteAudio(string audioUrl);
}