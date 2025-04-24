namespace Core.Entity.AnyContent;

public class ContentItem
{
    public int Id { get; set; }
    
    public int TopicId { get; set; }
    public Topic Topic { get; set; }
}