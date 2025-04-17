namespace Backend.Entity;

public class Course
{
    public int CourseId { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    
    public List<Topic> Topics { get; set; } = new();
}