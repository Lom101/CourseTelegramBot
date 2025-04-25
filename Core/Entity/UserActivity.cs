namespace Core.Entity;

public class UserActivity
{
    public int Id { get; set; }
    
    public int UserId { get; set; }
    public User User { get; set; }

    public DateTime ActivityDate { get; set; }
}