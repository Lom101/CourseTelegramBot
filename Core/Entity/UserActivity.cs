namespace Core.Entity;

public class UserActivity
{
    public int Id { get; set; }
    
    public int UserId { get; set; }
    public User User { get; set; }
    
    public string ActionType { get; set; } // "button_click", "link_click", "test_attempt"
    public string Details { get; set; }
    public DateTime ActivityDate { get; set; }
}