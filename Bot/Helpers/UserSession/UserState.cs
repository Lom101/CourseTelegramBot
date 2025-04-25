namespace Bot.Helpers.UserSession;

public enum UserState
{
    None,
    AwaitingFullName,
    AwaitingPhone,
    Authorized
}