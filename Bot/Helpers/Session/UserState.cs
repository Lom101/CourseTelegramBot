namespace Bot.Helpers.Session;

public enum UserState
{
    None,
    AwaitingFullName,
    AwaitingPhone,
    Authorized
}