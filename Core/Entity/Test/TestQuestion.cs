namespace Core.Entity.Test;

public class TestQuestion
{
    public int Id { get; set; }
    
    public int TestId { get; set; }
    public Test Test { get; set; }

    public string QuestionText { get; set; }
    public List<string> Options { get; set; }
    public int CorrectIndex { get; set; }
}