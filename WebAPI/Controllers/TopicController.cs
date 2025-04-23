using Backend.Dto.Topic.Request;
using Backend.Mapper;
using Core.Dto.Topic.Request;
using Core.Entity;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TopicController(ITopicRepository topicRepository) : ControllerBase
{
    [HttpGet("by-course/{id}")]
    public async Task<IActionResult> GetTopicByCourseId(int courseId)
    {
        var topics = await topicRepository.GetByCourseIdAsync(courseId);
        if (!topics.Any())
            return NotFound($"No topics found for course {courseId}");
        
        var dtos = topics.Select(TopicMapper.ToDto).ToList();
        return Ok(dtos);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetTopicById(int id)
    {
        var topic = await topicRepository.GetByIdAsync(id);
        if (topic == null)
            throw new KeyNotFoundException($"Topic with id {id} not found");

        var dto = TopicMapper.ToDto(topic);
        
        return Ok(dto);
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateTopic([FromBody] CreateTopicRequest topicDto) // Принимаем DTO
    {
        if (topicDto == null)
            return BadRequest("Invalid topic data");

        // Преобразуем DTO в сущность
        var topic = TopicMapper.ToEntity(topicDto);

        await topicRepository.AddAsync(topic);

        // Возвращаем только что созданный объект как DTO
        var createdDto = TopicMapper.ToDto(topic);
        return CreatedAtAction(nameof(GetTopicById), new { id = topic.Id }, createdDto);
    }
}