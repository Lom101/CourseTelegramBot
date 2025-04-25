using Core.Entity.Progress;
using Core.Entity.Test;

namespace Core.Entity;

public class Block
{
    public int Id { get; set; }

    public string Title { get; set; }

    public int FinalTestId { get; set; }
    public FinalTest FinalTest { get; set; } // Финальный тест для этого блока
    
    public List<Topic> Topics { get; set; } = new();
    public List<UserProgress> UserProgress { get; set; } = new();
}