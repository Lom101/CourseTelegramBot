using Core.Enum;

namespace Core.Entity;

public class UserActivity
{
    public int Id { get; set; }
    
    public int UserId { get; set; }
    public User User { get; set; }

    public UserActionType ActionType { get; set; }
    public string Metadata { get; set; } // например: JSON со ссылкой, названием кнопки и т.д.
    public DateTime ActivityDate { get; set; }
}