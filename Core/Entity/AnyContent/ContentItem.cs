namespace Core.Entity;

public class ContentItem
{
    public int Id { get; set; }
    
    public string Title { get; set; }
    public string Description { get; set; }
    
    public int TopicId { get; set; }
    public Topic Topic { get; set; }
    
    // Поле для указания порядка контента
    public int Order { get; set; } 
}