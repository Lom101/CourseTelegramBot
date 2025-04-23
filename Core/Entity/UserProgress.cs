namespace Core.Entity;

public class UserProgress
{
    public int Id { get; set; }
    
    public long UserId { get; set; }
    public User User { get; set; }
    
    public int ContentId { get; set; }
    public ContentItem Content { get; set; }
    
    public bool IsCompleted { get; set; }
    public DateTime? CompletionDate { get; set; }
}