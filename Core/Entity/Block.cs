
namespace Core.Entity;

public class Block
{
    public int Id { get; set; }

    public string Title { get; set; }

    public int TestId { get; set; }
    public global::Core.Entity.Test.Test Test { get; set; } // Финальный тест для этого курса
    
    public List<Topic> Topics { get; set; } = new();
    public List<UserProgress> UserProgress { get; set; } = new();
}