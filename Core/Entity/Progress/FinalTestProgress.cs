namespace Core.Entity;

public class FinalTestProgress : UserProgress
{
    public bool IsPassed { get; set; }
    public DateTime PassedAt { get; set; }
}