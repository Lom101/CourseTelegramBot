namespace Core.Entity;

public class Block
{
    public int Id { get; set; }
    
    public string Title { get; set; }
    
    public List<Topic> Topics { get; set; } = new();
}