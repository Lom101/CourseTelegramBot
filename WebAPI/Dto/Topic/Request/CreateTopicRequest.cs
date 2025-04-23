using System.ComponentModel;

namespace Core.Dto.Topic.Request;

public class CreateTopicRequest
{
    [DefaultValue("Введение в курс тим-лид")]
    public string Title { get; set; }

    [DefaultValue(1)]
    public int Order { get; set; }

    [DefaultValue(1)]
    public int CourseId { get; set; } 
}
