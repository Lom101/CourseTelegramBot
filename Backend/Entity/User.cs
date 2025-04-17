namespace Backend.Entity;

public class User
{
    public long UserId { get; set; }
    public long TelegramId { get; set; }
    public string PhoneNumber { get; set; }
    public string Email { get; set; }
    public string FullName { get; set; }
    public DateTime RegistrationDate { get; set; }
    public DateTime? LastActivity { get; set; }
    public bool IsBlocked { get; set; }
    public bool IsAdmin { get; set; }
}