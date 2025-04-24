namespace Core.Entity.Test;

public class TestQuestion
{
    public int Id { get; set; }
    
    public int TestId { get; set; }
    public global::Core.Entity.Test.Test Test { get; set; }

    public string QuestionText { get; set; }
    public ICollection<TestOption> Options { get; set; } = new List<TestOption>();  // Связь с вариантами ответов
    public int CorrectIndex { get; set; }
}