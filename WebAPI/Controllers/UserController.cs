using Backend.Mapper;
using Core.Dto.Request;
using Core.Entity;
using Core.Interfaces;
using Core.Model;
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
    public async Task<IActionResult> GetUserById(int id)
    {
        var user = await userRepository.GetByIdAsync(id);
        if (user == null)
            return NotFound($"User with id {id} not found");

        var dto = UserMapper.ToDto(user); 
        return Ok(dto);
    }
    
    /// <summary>
    /// Получить всех пользователей.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAllUsers()
    {
        var users = await userRepository.GetAllAsync();
        if (!users.Any())
            return NotFound("No users found");

        var dtos = users.Select(UserMapper.ToDto).ToList(); // Преобразуем все сущности в GetUserResponse
        return Ok(dtos);
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

        var dto = UserMapper.ToDto(user); 
        return Ok(dto);
    }
    
    /// <summary>
    /// Зарегистрировать нового пользователя.
    /// </summary>
    [HttpPost("add-new-user")]
    public async Task<IActionResult> AddNewUser([FromBody] CreateUserRequest request)
    {
        if (!ModelState.IsValid)
            throw new ArgumentException("Invalid user data");

        if (request.ChatId.HasValue)
        {
            var existingUser = await userRepository.GetByChatIdAsync(request.ChatId.Value);
            if (existingUser != null)
                throw new ArgumentException($"User with chat id {request.ChatId} already exists");
        }

        // Преобразуем DTO в сущность User
        var user = UserMapper.ToEntity(request);
        
        await userRepository.AddAsync(user);
        
        var dto = UserMapper.ToDto(user);
        return CreatedAtAction(nameof(GetUserById), new { id = user.Id }, dto);
    }
    
    /// <summary>
    /// Обновить данные пользователя.
    /// </summary>
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateUser(int id, [FromBody] UpdateUserRequest request)
    {
        if (request == null)
            return BadRequest("Invalid user data");
        
        if (request.Id != id)
            return BadRequest("Id does not match request id");

        var user = await userRepository.GetByIdAsync(id);
        if (user == null)
            return NotFound($"User with id {id} not found");
        
        // Обновляем данные пользователя
        user = UserMapper.ToEntity(user, request);
        await userRepository.UpdateAsync(user);

        // Возвращаем обновленного пользователя
        var dto = UserMapper.ToDto(user);
        return Ok(dto);
    }

    /// <summary>
    /// Заблокировать пользователя.
    /// </summary>
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
    /// Получить список пользователей с фильтрацией.
    /// </summary>
    [HttpGet("filter")]
    public async Task<ActionResult<IEnumerable<User>>> GetFilteredUsers([FromQuery] UserFilterModel filterRequest)
    {
        var users = await userRepository.GetFilteredUsersAsync(filterRequest);
        
        var dtos = users.Select(UserMapper.ToDto).ToList();
        return Ok(dtos);
    }
}
