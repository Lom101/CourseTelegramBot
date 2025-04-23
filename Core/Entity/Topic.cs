using Core.Entity.AnyContent;

namespace Core.Entity;

public class Topic
{
    public int Id { get; set; }
    
    public string Title { get; set; }
    
    public int BlockId { get; set; }
    public Block Block { get; set; }
    
    public List<ContentItem> ContentItems { get; set; } = new();
}