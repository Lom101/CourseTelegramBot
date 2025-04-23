namespace Core.Entity.AnyContent;

public class ContentItem
{
    public int Id { get; set; }
    
    public string Description { get; set; }
    
    public int TopicId { get; set; }
    public Topic Topic { get; set; }
    
    // Поле для указания порядка контента
    public int Order { get; set; } 
}