namespace Core.Entity.Test;

public class TestQuestion
{
    public int Id { get; set; }
    
    public int FinalTestId { get; set; }
    public FinalTest FinalTest { get; set; }

    public string QuestionText { get; set; }
    public IList<TestOption> Options { get; set; } = new List<TestOption>();  // Связь с вариантами ответов
    public int CorrectIndex { get; set; } // индекс правильного ответа
}