using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserProgressController : ControllerBase
{
    private readonly IUserProgressRepository _progressRepository;

    public UserProgressController(IUserProgressRepository progressRepository)
    {
        _progressRepository = progressRepository;
    }

    /// <summary>
    /// Получить полный прогресс пользователя по курсу
    /// </summary>
    [HttpGet("{userId}")]
    public async Task<IActionResult> GetUserProgressDetails(int userId)
    {
        var progressDetails = await _progressRepository.GetUserProgressDetailsAsync(userId);

        if (progressDetails == null)
        {
            return NotFound();
        }

        return Ok(progressDetails);
    }
}