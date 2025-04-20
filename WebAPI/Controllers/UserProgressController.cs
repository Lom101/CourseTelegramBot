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

    [HttpPost]
    public async Task<ActionResult<UserProgress>> Update([FromBody] UserProgress progress)
    {
        if (!ModelState.IsValid)
            throw new ArgumentException("Invalid progress data");

        var existing = await progressRepository.GetByUserAndContentAsync(progress.UserId, progress.ContentId);
        
        if (existing == null)
        {
            await progressRepository.AddAsync(progress);
            return CreatedAtAction(nameof(GetByUserId), new { userId = progress.UserId }, progress);
        }

        existing.IsCompleted = progress.IsCompleted;
        existing.CompletionDate = progress.CompletionDate;
        await progressRepository.UpdateAsync(existing);
        return Ok(existing);
    }
}