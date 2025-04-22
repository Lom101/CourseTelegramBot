using System.ComponentModel;

namespace Core.Dto.Request;

public class UpdateUserRequest
{
    public int Id { get; set; }  

    [DefaultValue("+79991234567")]
    public string? PhoneNumber { get; set; }

    [DefaultValue("updated@example.com")]
    public string? Email { get; set; }

    [DefaultValue("Новое Имя")]
    public string? FullName { get; set; }

    [DefaultValue(false)]
    public bool? IsBlocked { get; set; }

    [DefaultValue(false)]
    public bool? IsAdmin { get; set; }
}