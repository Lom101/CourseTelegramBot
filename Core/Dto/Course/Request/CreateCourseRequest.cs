using System.ComponentModel;

namespace Core.Dto.Course.Request;

public class CreateCourseRequest
{
    [DefaultValue("Основы программирования")]
    public string Title { get; set; }

    [DefaultValue("Курс для начинающих разработчиков")]
    public string Description { get; set; }
}