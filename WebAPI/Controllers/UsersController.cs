using Core.Entity;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

/// <summary>
/// Контроллер для управления пользователями.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class UsersController(IUserRepository userRepository) : ControllerBase
{
    /// <summary>
    /// Получить пользователя по ID.
    /// </summary>
    /// <param name="id">ID пользователя.</param>
    /// <returns>Пользователь с заданным ID.</returns>
    [HttpGet("{id:int}")]
    public async Task<ActionResult<User>> GetById(int id)
    {
        var user = await userRepository.GetByIdAsync(id);
        return user ?? throw new KeyNotFoundException($"User with id {id} not found");
    }
    
    /// <summary>
    /// Получить всех пользователей.
    /// </summary>
    /// <returns>Список всех пользователей.</returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<User>>> GetAll()
    {
        var users = await userRepository.GetAllAsync();
        return Ok(users);
    }
    
    /// <summary>
    /// Получить пользователя по ChatId в Telegram.
    /// </summary>
    /// <param name="chatId">Chat ID пользователя.</param>
    /// <returns>Пользователь с заданным ChatId.</returns>
    [HttpGet("by-chat/{chatId:long}")]
    public async Task<ActionResult<User>> GetByChatId(long chatId)
    {
        var user = await userRepository.GetByChatIdAsync(chatId);
        return user ?? throw new KeyNotFoundException($"User with chat id {chatId} not found");
    }
    
    /// <summary>
    /// Зарегистрировать нового пользователя.
    /// </summary>
    /// <param name="user">Данные нового пользователя.</param>
    /// <returns>Созданный пользователь.</returns>
    [HttpPost("register")]
    public async Task<ActionResult<User>> Register([FromBody] User user)
    {
        if (!ModelState.IsValid)
            throw new ArgumentException("Invalid user data");

        var existingUser = await userRepository.GetByChatIdAsync(user.ChatId);
        if (existingUser != null)
            throw new ArgumentException($"User with chat id {user.ChatId} already exists");

        await userRepository.AddAsync(user);
        return CreatedAtAction(nameof(GetByChatId), new { chatId = user.ChatId }, user);
    }
    
    
}