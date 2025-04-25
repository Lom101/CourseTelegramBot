namespace Core.Entity.Progress;

public class FinalTestProgress : UserProgress
{
    public bool IsPassed { get; set; }
    public int CorrectAnswersCount { get; set; } // Количество правильных ответов
    public DateTime PassedAt { get; set; }
}