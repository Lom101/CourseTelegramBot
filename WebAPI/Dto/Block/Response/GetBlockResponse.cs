using Backend.Dto.Topic.Response;
using Core.Entity.Test;

namespace Backend.Dto.Block.Response;

public class GetBlockResponse
{
    public int Id { get; set; }
    public string Title { get; set; }
    public int FinalTestId { get; set; }
    public FinalTest FinalTest { get; set; }
    public List<GetTopicResponse> Topics { get; set; } = new();
}