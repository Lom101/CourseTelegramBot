using Core.Entity;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserProgressController(IUserProgressRepository progressRepository) : ControllerBase
{
    [HttpGet("{userId:long}")]
    public async Task<ActionResult<IEnumerable<UserProgress>>> GetByUserId(long userId)
    {
        var progress = await progressRepository.GetByUserIdAsync(userId);
        return Ok(progress);
    }
   
}