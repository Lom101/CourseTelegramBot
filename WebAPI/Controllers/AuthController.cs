using Backend.Dto.Auth.Request;
using Backend.Dto.Auth.Response;
using Backend.Service;
using Core.Interfaces;
using Infrastructure.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

/// <summary>
/// Контроллер для авторизации администраторов.
/// </summary>
[ApiController]
[Route("api/[controller]")] 
public class AuthController(IUserRepository userRepository, JwtService jwtService) : ControllerBase
{
    /// <summary>
    /// Авторизация администратора.
    /// </summary>
    /// <param name="request">Данные для входа администратора (email и пароль).</param>
    /// <returns>JWT-токен при успешной авторизации.</returns>
    /// <response code="200">Успешная авторизация. Возвращает JWT токен.</response>
    /// <response code="401">Неверный логин или пароль, либо доступ запрещен.</response>
    [AllowAnonymous]
    [HttpPost("admin-login")]
    public async Task<IActionResult> AdminLogin([FromBody] AdminLoginRequest request)
    {
        var user = await userRepository.GetByEmailAsync(request.Email);
        if (user is null || !user.IsAdmin || user.IsBlocked)
            return Unauthorized("Неверный логин или доступ запрещен");

        if (!PasswordHasher.VerifyPassword(request.Password, user.PasswordHash))
            return Unauthorized("Неверный пароль");

        var token = jwtService.GenerateToken(user.Email);
        return Ok(new TokenResponse { Token = token });
    }

    /// <summary>
    /// Проверка доступа администратора.
    /// </summary>
    /// <returns>Простое сообщение, если пользователь авторизован как администратор.</returns>
    /// <response code="200">Пользователь авторизован и имеет доступ.</response>
    /// <response code="401">Нет авторизации или отсутствует токен.</response>
    [Authorize]
    [HttpGet("check")]
    public IActionResult GetAdminData()
    {
        return Ok("Вы админ!");
    }
}
