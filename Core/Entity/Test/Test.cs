namespace Core.Entity.Test;

public class Test
{
    public int Id { get; set; }
    
    public string Title { get; set; }
    public ICollection<TestQuestion> Questions { get; set; } = new List<TestQuestion>();
}