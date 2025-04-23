using Core.Entity;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

/// <summary>
/// Контроллер для получения статистики активности пользователей
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class UserActivityController(IUserActivityRepository activityRepository) : ControllerBase
{
    /// <summary>
    /// Получить активность пользователя
    /// </summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetUserActivityByUserId(int userId)
    {
        var activities = await activityRepository.GetByUserIdAsync(userId);
        return Ok(activities);
    }
}