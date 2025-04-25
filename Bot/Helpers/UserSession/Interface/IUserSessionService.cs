namespace Bot.Helpers.UserSession.Interface;

public interface IUserSessionService
{
    UserSession GetOrCreate(long chatId);
    bool TryGet(long chatId, out UserSession? session);
    void SetState(long chatId, UserState state);
    void Clear(long chatId);
}