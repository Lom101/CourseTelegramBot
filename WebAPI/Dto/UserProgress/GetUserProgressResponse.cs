namespace Backend.Dto.UserProgress;

public class GetUserProgressResponse
{
    public int UserId { get; set; }
    public List<GetBlockProgressResponse> BlockProgresses { get; set; }
}
