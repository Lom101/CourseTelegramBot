namespace Core.Entity.Test;

public class TestOption
{
    public int Id { get; set; }

    public int TestQuestionId { get; set; }
    public TestQuestion TestQuestion { get; set; }

    public string OptionText { get; set; }
}