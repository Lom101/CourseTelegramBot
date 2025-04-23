using System.ComponentModel;

namespace Backend.Dto.Topic.Request;

public class UpdateTopicRequest
{
    public int Id { get; set; } 

    [DefaultValue("Обновленное название темы")]
    public string? Title { get; set; }
}