using System.ComponentModel;

namespace Core.Dto.Request;

public class CreateUserRequest
{
    [DefaultValue(123456789)]
    public long ChatId { get; set; }

    [DefaultValue("+79991234567")]
    public string PhoneNumber { get; set; }

    [DefaultValue("user@example.com")]
    public string? Email { get; set; }

    [DefaultValue("Иван Иванов")]
    public string FullName { get; set; }
}