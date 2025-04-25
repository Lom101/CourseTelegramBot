using Backend.Dto.FinalTest.Request;
using Backend.Mapper;
using Core.Entity.Test;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FinalTestController : ControllerBase
{
    private readonly IFinalTestRepository _finalTestRepository;

    public FinalTestController(IFinalTestRepository finalTestRepository)
    {
        _finalTestRepository = finalTestRepository;
    }
    
    // Получение всех финальных тестов
    [HttpGet]
    public async Task<IActionResult> GetAllFinalTests()
    {
        var finalTests = await _finalTestRepository.GetAllAsync();

        if (finalTests == null || !finalTests.Any())
        {
            return NotFound("No final tests found.");
        }

        // Используем маппер для преобразования сущностей в DTO
        var result = finalTests.Select(f => FinalTestMapper.ToDto(f)).ToList();

        return Ok(result);
    }
    
    // Получение финального теста по ID
    [HttpGet("{id}")]
    public async Task<IActionResult> GetFinalTestById(int id)
    {
        var finalTest = await _finalTestRepository.GetByIdAsync(id);

        if (finalTest == null)
        {
            return NotFound($"Final test with ID {id} not found.");
        }

        // Используем маппер для преобразования сущности в DTO
        var result = FinalTestMapper.ToDto(finalTest);

        return Ok(result);
    }

    // Создание финального теста с вопросами и вариантами
    [HttpPost]
    public async Task<IActionResult> CreateFinalTest([FromBody] CreateFinalTestRequest request)
    {
        if (request == null || request.Questions == null || !request.Questions.Any())
        {
            return BadRequest("Invalid request data.");
        }

        // Маппинг DTO в сущности с использованием маппера
        var finalTest = FinalTestMapper.ToEntity(request);

        // Создаем тест с вопросами и вариантами
        var createdTestId = await _finalTestRepository.CreateAsync(finalTest);

        // Возвращаем информацию о созданном тесте
        return Ok(new { Id = createdTestId });
    }   
}

