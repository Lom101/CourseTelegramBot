namespace Bot.Helpers.Session;

public class UserSession
{
    public string? FullName { get; set; }
    public string? PhoneNumber { get; set; }
    public UserState State { get; set; } = UserState.None;
}