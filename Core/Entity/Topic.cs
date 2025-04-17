namespace Core.Entity;

public class Topic
{
    public int Id { get; set; }
    
    public string Title { get; set; }
    public int Order { get; set; }
    
    public int CourseId { get; set; }
    public Course Course { get; set; }
    
    public List<ContentItem> ContentItems { get; set; } = new();
}