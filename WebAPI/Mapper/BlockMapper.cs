using Backend.Dto.Block.Request;
using Backend.Dto.Block.Response;
using Core.Entity;

namespace Backend.Mapper;

public class BlockMapper
{
    public static Block ToEntity(CreateBlockRequest request)
    {
        return new Block
        {
            Title = request.Title,
            FinalTestId = request.FinalTestId,
        };
    }

    public static void ToEntity(Block entity, UpdateBlockRequest request)
    {
        if (request.Title != null) entity.Title = request.Title;
        if (request.FinalTestId.HasValue) entity.FinalTestId = request.FinalTestId.Value;
    }

    public static GetBlockResponse ToDto(Block entity)
    {
        return new GetBlockResponse
        {
            Id = entity.Id,
            Title = entity.Title,
            FinalTestId = entity.FinalTestId,
        };
    }
}