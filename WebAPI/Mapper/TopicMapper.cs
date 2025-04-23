using Backend.Dto.Topic.Request;
using Core.Dto.Topic.Request;
using Core.Dto.Topic.Response;
using Core.Entity;

namespace Backend.Mapper;

public class TopicMapper
{
    public static GetTopicResponse ToDto(Topic topic)
    {
        return new GetTopicResponse
        {
            Id = topic.Id,
            Title = topic.Title,
            Order = topic.Order,
        };
    }

    public static Topic ToEntity(CreateTopicRequest request)
    {
        return new Topic
        {
            Title = request.Title,
            CourseId = request.CourseId,
            Order = request.Order,
        };
    }
}