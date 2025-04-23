namespace Core.Entity.AnyContent;

public class AudioContent : ContentItem
{
    public string AudioUrl { get; set; }
    public string AudioTitle { get; set; }
    public string FileName { get; set; } // <- Название файла, например "lesson1.mp3"
}