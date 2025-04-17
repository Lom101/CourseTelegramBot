namespace Backend.Entity;

public class ContentItem
{
    public int ContentId { get; set; }
    public int TopicId { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public int Order { get; set; }
    
    public Topic Topic { get; set; }
}