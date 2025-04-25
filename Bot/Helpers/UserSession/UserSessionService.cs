using System.Collections.Concurrent;
using Bot.Helpers.UserSession.Interface;

namespace Bot.Helpers.UserSession;

public class UserSessionService : IUserSessionService
{
    private readonly ConcurrentDictionary<long, Helpers.UserSession.UserSession> _sessions = new();

    public Helpers.UserSession.UserSession GetOrCreate(long chatId)
    {
        return _sessions.GetOrAdd(chatId, new Helpers.UserSession.UserSession());
    }

    public bool TryGet(long chatId, out Helpers.UserSession.UserSession? session)
    {
        return _sessions.TryGetValue(chatId, out session);
    }

    public void SetState(long chatId, UserState state)
    {
        var session = GetOrCreate(chatId);
        session.State = state;
    }

    public void Clear(long chatId)
    {
        _sessions.TryRemove(chatId, out _);
    }
}
