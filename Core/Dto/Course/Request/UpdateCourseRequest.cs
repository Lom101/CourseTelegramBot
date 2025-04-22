using System.ComponentModel;

namespace Core.Dto.Course.Request;

public class UpdateCourseRequest
{
    public int Id { get; set; }
    
    [DefaultValue("Новое название курса")]
    public string? Title { get; set; }
    
    [DefaultValue("Обновленное описание")]
    public string? Description { get; set; }
}