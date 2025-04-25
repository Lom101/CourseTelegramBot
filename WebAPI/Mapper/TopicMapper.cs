using Backend.Dto.Topic.Request;
using Backend.Dto.Topic.Response;
using Core.Entity;

namespace Backend.Mapper;

public class TopicMapper
{
    public static GetTopicResponse ToDto(Topic topic)
    {
        return new GetTopicResponse
        {
            Id = topic.Id,
            Title = topic.Title
        };
    }

    public static Topic ToEntity(CreateTopicRequest request)
    {
        return new Topic
        {
            Title = request.Title,
            BlockId = request.BlockId
        };
    }
    
    public static Topic ToEntity(Topic topic, UpdateTopicRequest request)
    {
        if (request.Title != null) topic.Title = request.Title;
        return topic;
    }
}