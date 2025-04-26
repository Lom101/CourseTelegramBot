using System.ComponentModel;

namespace Backend.Dto.User.Request;

public class CreateUserRequest
{
    [DefaultValue("+79991234567")]
    public string PhoneNumber { get; set; }

    [DefaultValue("user@example.com")]
    public string? Email { get; set; }

    [DefaultValue("Иван Иванов")]
    public string? FullName { get; set; }
}