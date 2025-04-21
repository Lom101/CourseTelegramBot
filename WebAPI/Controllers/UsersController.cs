using Core.Entity;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController(IUserRepository userRepository) : ControllerBase
{
    [HttpGet("by-chat/{chatId:long}")]
    public async Task<ActionResult<User>> GetByChatId(long chatId)
    {
        var user = await userRepository.GetByChatIdAsync(chatId);
        return user ?? throw new KeyNotFoundException($"User with chat id {chatId} not found");
    }

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
    
    // TODO: можно добавить получение по id просто, параллельно с получением по chatId
}