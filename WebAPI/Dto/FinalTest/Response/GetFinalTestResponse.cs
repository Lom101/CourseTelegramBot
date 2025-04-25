namespace Backend.Dto.FinalTest.Response;

public class GetFinalTestResponse
{
    public int Id { get; set; }
    public string Title { get; set; }
    public List<GetTestQuestionResponse> Questions { get; set; }
}

public class GetTestQuestionResponse
{
    public int Id { get; set; }
    public string QuestionText { get; set; }
    public int CorrectIndex { get; set; }
    public List<GetTestOptionResponse> Options { get; set; }
}

public class GetTestOptionResponse
{
    public int Id { get; set; }
    public string OptionText { get; set; }
}