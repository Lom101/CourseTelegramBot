using Core.Enum;

namespace Core.Entity;

public class UserProgress
{
    public int Id { get; set; }
    
    public int UserId { get; set; }
    public User User { get; set; }

    public int BlockId { get; set; }
    public Block Block { get; set; }
    
    public DateTime UpdatedAt { get; set; }
}