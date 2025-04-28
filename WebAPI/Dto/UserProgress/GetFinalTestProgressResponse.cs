namespace Backend.Dto.UserProgress;

public class GetFinalTestProgressResponse
{
    public bool IsPassed { get; set; }
    public int CorrectAnswersCount { get; set; }
    public DateTime PassedAt { get; set; }
}