using Backend.Dto.FinalTest.Response;
using Backend.Dto.Topic.Response;

namespace Backend.Dto.Block.Response;

public class GetBlockResponse
{
    public int Id { get; set; }
    public string Title { get; set; }
    public int FinalTestId { get; set; }
    public GetFinalTestResponse FinalTest { get; set; }
    public List<GetTopicResponse> Topics { get; set; } = new();
}