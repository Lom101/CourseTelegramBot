namespace Backend.Service.Interfaces;

public interface IJwtService
{
    /// <summary>
    /// Генерирует JWT-токен для указанного email.
    /// </summary>
    string GenerateToken(string email);
}