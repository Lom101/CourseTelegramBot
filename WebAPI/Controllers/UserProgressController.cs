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
    [HttpGet("{userId}")]
    public async Task<IActionResult> GetUserProgressByUserId(int userId)
    {
        //var progress = await progressRepository.GetByUserIdAsync(userId);
        //return Ok(progress);
        
        return Ok();
    }
    
    /*
    // Получить прогресс по теме
    [HttpGet("topic/{userId}/{topicId}")]
    public async Task<ActionResult<TopicProgress>> GetTopicProgress(int userId, int topicId)
    {
        var topicProgress = await progressRepository.GetTopicProgressAsync(userId, topicId);

        if (topicProgress == null)
        {
            return NotFound();
        }

        return Ok(topicProgress);
    }
    
    // Получить прогресс по тесту
    [HttpGet("finaltest/{userId}/{blockId}")]
    public async Task<ActionResult<FinalTestProgress>> GetFinalTestProgress(int userId, int blockId)
    {
        var finalTestProgress = await progressRepository.GetFinalTestProgressAsync(userId, blockId);

        if (finalTestProgress == null)
        {
            return NotFound();
        }

        return Ok(finalTestProgress);
    }

    // Получить прогресс по завершению курса
    [HttpGet("coursecompletion/{userId}/{blockId}")]
    public async Task<ActionResult<BlockCompletionProgress>> GetCourseCompletionProgress(int userId, int blockId)
    {
        var courseCompletionProgress = await progressRepository.GetCourseCompletionProgressAsync(userId, blockId);

        if (courseCompletionProgress == null)
        {
            return NotFound();
        }

        return Ok(courseCompletionProgress);
    }
    
    // Сохранить прогресс по теме
    [HttpPost("topic")]
    public async Task<ActionResult> SaveTopicProgress(TopicProgress progress)
    {
        await progressRepository.SaveTopicProgressAsync(progress);
        return NoContent();
    }

    // Сохранить прогресс по тесту
    [HttpPost("finaltest")]
    public async Task<ActionResult> SaveFinalTestProgress(FinalTestProgress progress)
    {
        await progressRepository.SaveFinalTestProgressAsync(progress);
        return NoContent();
    }

    // Сохранить прогресс по завершению курса
    [HttpPost("coursecompletion")]
    public async Task<ActionResult> SaveCourseCompletionProgress(BlockCompletionProgress progress)
    {
        await progressRepository.SaveCourseCompletionProgressAsync(progress);
        return NoContent();
    }*/
}