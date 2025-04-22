using Core.Dto.User.Request;
using Core.Entity;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

/// <summary>
/// Контроллер для управления пользователями.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class UserController(IUserRepository userRepository) : ControllerBase
{
    /// <summary>
    /// Получить пользователя по ID.
    /// </summary>
    [HttpGet("{id}")]
    public async Task<IActionResult>  GetUserById(int id)
    {
        var user = await userRepository.GetByIdAsync(id);
        if (user == null)
            return NotFound($"User with id {id} not found");

        return Ok(user);
    }
    
    /// <summary>
    /// Получить всех пользователей.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAllUsers()
    {
        var users = await userRepository.GetAllAsync();
        return Ok(users);
    }

    /// <summary>
    /// Получить пользователя по ChatId в Telegram.
    /// </summary>
    [HttpGet("by-chat")]
    public async Task<IActionResult> GetUserByChatId([FromQuery] long chatId)
    {
        var user = await userRepository.GetByChatIdAsync(chatId);
        if (user == null)
            return NotFound($"User with chat id {chatId} not found");

        return Ok(user);
    }
    
    /// <summary>
    /// Зарегистрировать нового пользователя.
    /// </summary>
    [HttpPost("add-new-user")]
    public async Task<IActionResult> AddNewUser([FromBody] User user)
    {
        if (!ModelState.IsValid)
            throw new ArgumentException("Invalid user data");

        var existingUser = await userRepository.GetByChatIdAsync((long)user.ChatId);
        if (existingUser != null)
            throw new ArgumentException($"User with chat id {user.ChatId} already exists");

        await userRepository.AddAsync(user);
        return CreatedAtAction(nameof(GetUserByChatId), new { chatId = user.ChatId }, user);
    }

    /// <summary>
    /// Обновить данные пользователя.
    /// </summary>
    /// <param name="id">ID пользователя.</param>
    /// <param name="updatedUser">Обновлённые данные пользователя.</param>
    /// <returns>Обновлённый пользователь.</returns>
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateUser(int id, [FromBody] User updatedUser)
    {
        var user = await userRepository.GetByIdAsync(id);
        if (user == null)
            return NotFound($"User with id {id} not found");

        user.FullName = updatedUser.FullName;
        user.Email = updatedUser.Email;
        user.PhoneNumber = updatedUser.PhoneNumber;
        user.IsBlocked = updatedUser.IsBlocked;
        user.IsAdmin = updatedUser.IsAdmin;

        await userRepository.UpdateAsync(user);
        return Ok(user);
    }

    /// <summary>
    /// Заблокировать пользователя.
    /// </summary>
    /// <param name="id">ID пользователя.</param>
    [HttpPost("{id}/block")]
    public async Task<IActionResult> BlockUser(int id)
    {
        var user = await userRepository.GetByIdAsync(id);
        if (user == null)
            return NotFound($"User with id {id} not found");

        user.IsBlocked = true;
        await userRepository.UpdateAsync(user);
        return Ok("Пользователь заблокирован");
    }

    /// <summary>
    /// Разблокировать пользователя.
    /// </summary>
    /// <param name="id">ID пользователя.</param>
    [HttpPost("{id}/unblock")]
    public async Task<IActionResult> UnblockUser(int id)
    {
        var user = await userRepository.GetByIdAsync(id);
        if (user == null)
            return NotFound($"User with id {id} not found");

        user.IsBlocked = false;
        await userRepository.UpdateAsync(user);
        return Ok("Пользователь разблокирован");
    }
    
    /// <summary>
    /// Получить список пользователей с возможностью фильтрации.
    /// </summary>
    /// <param name="filterRequest">Параметры для фильтрации.</param>
    /// <returns>Список пользователей.</returns>
    [HttpGet("filter")]
    public async Task<ActionResult<IEnumerable<User>>> GetFilteredUsers([FromQuery] UserFilterRequest filterRequest)
    {
        var users = await userRepository.GetFilteredUsersAsync(filterRequest);
        return Ok(users);
    }
}
