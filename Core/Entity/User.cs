namespace Core.Entity;

public class User
{
    public int Id { get; set; }
    public long ChatId { get; set; }
    public string PhoneNumber { get; set; }
    public string Email { get; set; }
    public string FullName { get; set; }
    public DateTime RegistrationDate { get; set; }
    public DateTime? LastActivity { get; set; }
    public bool IsBlocked { get; set; }
    public bool IsAdmin { get; set; }
}