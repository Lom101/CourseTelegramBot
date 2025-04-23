using Core.Enum;

namespace Core.Entity;

public class UserProgress
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public User User { get; set; }

    public ProgressType Type { get; set; }
    public int TargetId { get; set; } // ID темы или теста (в зависимости от типа)

    public bool IsCompleted { get; set; }
    public DateTime? CompletionDate { get; set; }
}