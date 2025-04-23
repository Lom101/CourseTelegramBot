using Core.Entity;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;


[ApiController]
[Route("api/[controller]")]
public class UserActivityController(IUserActivityRepository activityRepository) : ControllerBase
{
    [HttpGet("{id}")]
    public async Task<IActionResult> GetUserActivityByUserId(int userId)
    {
        var activities = await activityRepository.GetByUserIdAsync(userId);
        return Ok(activities);
    }
}