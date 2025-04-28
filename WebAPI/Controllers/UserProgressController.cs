using Backend.Dto.UserProgress;
using Backend.Mapper;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserProgressController : ControllerBase
{
    private readonly IUserProgressRepository _progressRepository;
    private readonly IUserRepository _userRepository;

    public UserProgressController(IUserProgressRepository progressRepository, IUserRepository userRepository)
    {
        _progressRepository = progressRepository;
        _userRepository = userRepository;
    }
    
    /// <summary>
    /// Получить прогресс всех пользователей по курсу
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAllUsersProgress()
    {
        var users = await _userRepository.GetAllAsync();  // Получаем всех пользователей
        var usersProgress = new List<GetUserProgressResponse>();

        foreach (var user in users)
        {
            var progressDetails = await _progressRepository.GetUserProgressDetailsAsync(user.Id);  // Получаем прогресс пользователя
            if (progressDetails != null)
            {
                var progressResponse = progressDetails.ToGetUserProgressResponse(user);
                usersProgress.Add(progressResponse);
            }
        }

        return Ok(usersProgress);  // Возвращаем список прогресса всех пользователей
    }

    // /// <summary>
    // /// Получить полный прогресс пользователя по курсу
    // /// </summary>
    // [HttpGet("{userId}")]
    // public async Task<IActionResult> GetUserProgressDetails(int userId)
    // {
    //     var progressDetails = await _progressRepository.GetUserProgressDetailsAsync(userId);
    //
    //     if (progressDetails == null)
    //     {
    //         return NotFound();
    //     }
    //
    //     // Маппим сущности в DTO
    //     var progressResponse = progressDetails.ToGetUserProgressResponse();
    //
    //     return Ok(progressResponse);
    // }
}