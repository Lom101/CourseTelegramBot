﻿namespace Infrastructure.Helpers;

public static class PasswordHasher
{
    // Хеширует пароль
    public static string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }

    // Проверяет совпадение пароля и хеша
    public static bool VerifyPassword(string password, string hashedPassword)
    {
        return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
    }
}