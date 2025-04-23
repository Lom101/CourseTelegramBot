using System.ComponentModel;

namespace Core.Dto.Topic.Request;

public class UpdateTopicRequest
{
    public int Id { get; set; } 

    [DefaultValue("Обновленное название темы")]
    public string? Title { get; set; }

    [DefaultValue(2)]
    public int? Order { get; set; }

    [DefaultValue(1)]
    public int? CourseId { get; set; }
}