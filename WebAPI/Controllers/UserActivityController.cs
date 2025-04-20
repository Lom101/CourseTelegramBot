using Core.Entity;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;


[ApiController]
[Route("api/[controller]")]
public class UserActivityController(IUserActivityRepository activityRepository) : ControllerBase
{
    [HttpGet("{userId:long}")]
    public async Task<ActionResult<IEnumerable<UserActivity>>> GetByUserId(long userId)
    {
        var activities = await activityRepository.GetByUserIdAsync(userId);
        return Ok(activities);
    }

    [HttpPost]
    public async Task<ActionResult<UserActivity>> LogActivity([FromBody] UserActivity activity)
    {
        if (!ModelState.IsValid)
            throw new ArgumentException("Invalid activity data");

        await activityRepository.AddAsync(activity);
        return CreatedAtAction(nameof(GetByUserId), new { userId = activity.UserId }, activity);
    }
}