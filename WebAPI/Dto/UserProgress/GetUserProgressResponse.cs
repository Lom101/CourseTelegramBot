namespace Backend.Dto.UserProgress;

public class GetUserProgressResponse
{
    public int UserId { get; set; }
    public string FullName { get; set; }  // Имя пользователя
    public string Email { get; set; }     // Email пользователя
    public List<GetBlockProgressResponse> BlockProgresses { get; set; }
}
