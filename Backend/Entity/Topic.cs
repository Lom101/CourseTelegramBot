namespace Backend.Entity;

public class Topic
{
    public int TopicId { get; set; }
    public int CourseId { get; set; }
    public string Title { get; set; }
    public int Order { get; set; }
    
    // Навигационные свойства
    public Course Course { get; set; }
    public List<ContentItem> ContentItems { get; set; } = new();
   
}