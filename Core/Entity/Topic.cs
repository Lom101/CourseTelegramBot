using Core.Entity.AnyContent;

namespace Core.Entity;

public class Topic
{
    public int Id { get; set; }
    
    public string Title { get; set; }
    
    public int BlockId { get; set; }
    public Block Block { get; set; }
    
    // ссылка на сайт Longread
    public string? LongreadUrl { get; set; } // TODO: запрашивать url при добавлении темы
    
    public List<ContentItem> ContentItems { get; set; } = new();
}