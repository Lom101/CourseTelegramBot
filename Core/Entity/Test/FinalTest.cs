namespace Core.Entity.Test;

public class FinalTest
{
    public int Id { get; set; }
    public string Title { get; set; }
    public IList<TestQuestion> Questions { get; set; } = new List<TestQuestion>();
}