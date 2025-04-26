namespace Backend.Dto.Block.Request;

public class UpdateBlockRequest
{
    public int Id { get; set; }
    public string? Title { get; set; }    
    public int? FinalTestId { get; set; }
}