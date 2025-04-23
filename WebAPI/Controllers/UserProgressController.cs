using Core.Entity;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

/// <summary>
/// Контроллер для статистикой прогресса пользователя
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class UserProgressController(IUserProgressRepository progressRepository) : ControllerBase
{
    /// <summary>
    /// Получить прогресс пользователя по курсу
    /// </summary>
    [HttpGet("{userId:long}")]
    public async Task<IActionResult> GetUserProgressByUserId(int userId)
    {
        var progress = await progressRepository.GetByUserIdAsync(userId);
        return Ok(progress);
    }
}