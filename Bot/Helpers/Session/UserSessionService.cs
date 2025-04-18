using System.Collections.Concurrent;
using Bot.Helpers.Session.Interface;

namespace Bot.Helpers.Session;

public class UserSessionService : IUserSessionService
{
    private readonly ConcurrentDictionary<long, UserSession> _sessions = new();

    public UserSession GetOrCreate(long chatId)
    {
        return _sessions.GetOrAdd(chatId, new UserSession());
    }

    public bool TryGet(long chatId, out UserSession? session)
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
