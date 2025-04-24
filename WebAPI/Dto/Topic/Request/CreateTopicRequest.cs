using System.ComponentModel;

namespace Backend.Dto.Topic.Request;

public class CreateTopicRequest
{
    [DefaultValue("Введение в курс тим-лид")]
    public string Title { get; set; }

    [DefaultValue(1)]
    public int BlockId { get; set; } 
}
