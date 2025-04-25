using System.Collections.Concurrent;
using Bot.Helpers.TestSession.Interface;
using Core.Entity.Test;

namespace Bot.Helpers.TestSession;

public class FinalTestSessionService : IFinalTestSessionService
{
    private readonly ConcurrentDictionary<long, FinalTestSession> _sessions = new();

    public void StartSession(long userId, FinalTest test)
    {
        _sessions[userId] = new FinalTestSession
        {
            Test = test,
            CurrentQuestionIndex = 0,
            SelectedOptionIndices = new List<int>()
        };
    }

    public bool TryGetSession(long userId, out FinalTestSession session)
    {
        return _sessions.TryGetValue(userId, out session);
    }

    public void SaveAnswer(long userId, int answerIndex)
    {
        if (_sessions.TryGetValue(userId, out var session))
        {
            session.SelectedOptionIndices.Add(answerIndex);
            session.CurrentQuestionIndex++;
        }
    }

    public void ClearSession(long userId)
    {
        _sessions.TryRemove(userId, out _);
    }
}
