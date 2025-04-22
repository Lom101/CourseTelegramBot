namespace Core.Entity;

public class User
{
    public int Id { get; set; }
    public long? ChatId { get; set; } // админ не знает какой chatid у юзера
    public string? PhoneNumber { get; set; } // админ должен обязательно ввести при регистрации юзера в админ панели
    public string Email { get; set; } 
    public string? FullName { get; set; }
    public DateTime RegistrationDate { get; set; }
    public DateTime? LastActivity { get; set; }
    public bool IsBlocked { get; set; }
    public bool IsAdmin { get; set; }
    
    public string? PasswordHash { get; set; } // Только для админов, вход по логину/паролю
}