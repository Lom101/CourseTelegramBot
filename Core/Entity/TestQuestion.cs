namespace Core.Entity;

public class TestQuestion
{
    public int Id { get; set; }
    public string QuestionText { get; set; }
    public List<string> Options { get; set; } // Варианты ответа
    public int CorrectIndex { get; set; } // Индекс правильного варианта
}