namespace Core.Entity;

public class VideoContent : ContentItem
{
    public string VideoUrl { get; set; }
    public string ThumbnailUrl { get; set; }
    public TimeSpan Duration { get; set; }
}