namespace Backend.Dto.FinalTest.Request;

public class CreateFinalTestRequest
{
    public string Title { get; set; }
    public List<TestQuestionCreateRequest> Questions { get; set; } = new();
}

public class TestQuestionCreateRequest
{
    public string QuestionText { get; set; }
    public List<TestOptionCreateRequest> Options { get; set; }
    public int CorrectIndex { get; set; }
}

public class TestOptionCreateRequest
{
    public string OptionText { get; set; }
}