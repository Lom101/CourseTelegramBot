using Core.Entity.Test;

namespace Bot.Helpers.TestSession.Interface;

public interface IFinalTestSessionService
{
    void StartSession(long userId, FinalTest test);
    bool TryGetSession(long userId, out FinalTestSession session);
    void SaveAnswer(long userId, int answerIndex);
    void ClearSession(long userId);
}
