using Core.Entity.Test;

namespace Bot.Helpers.TestSession;

public class FinalTestSession
{
    public FinalTest Test { get; set; }
    public int CurrentQuestionIndex { get; set; } = 0;
    public List<int> SelectedOptionIndices { get; set; } = new();
}
