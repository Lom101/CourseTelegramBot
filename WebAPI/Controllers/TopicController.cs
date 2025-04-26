using Backend.Dto.Topic.Request;
using Backend.Mapper;
using Core.Entity;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TopicController(ITopicRepository topicRepository) : ControllerBase
{
    /// <summary>
    /// Получить тему по ID.
    /// </summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetTopicById(int id)
    {
        var topic = await topicRepository.GetByIdAsync(id);
        if (topic == null)
            throw new KeyNotFoundException($"Topic with id {id} not found");

        var dto = TopicMapper.ToDto(topic);
        return Ok(dto);
    }
    
    /// <summary>
    /// Получить все темы по идентификатору главы.
    /// </summary>
    [HttpGet("all-by-block/{blockId}")]
    public async Task<IActionResult> GetTopicsByBlockId(int blockId)
    {
        var topics = await topicRepository.GetByBlockIdAsync(blockId);
        if (!topics.Any())
            return NotFound($"No topics found for block {blockId}");
        
        var dtos = topics.Select(TopicMapper.ToDto).ToList();
        return Ok(dtos);
    }
    
    /// <summary>
    /// Создать новую тему.
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> CreateTopic([FromBody] CreateTopicRequest topicDto)
    {
        if (topicDto == null)
            return BadRequest("Invalid topic data");
        
        var topic = TopicMapper.ToEntity(topicDto);
        await topicRepository.AddAsync(topic);

        var dto = TopicMapper.ToDto(topic);
        return CreatedAtAction(nameof(GetTopicById), new { id = topic.Id }, dto);
    }
    
    /// <summary>
    /// Изменить существующую тему.
    /// </summary>
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateTopic(int id, [FromBody] UpdateTopicRequest request)
    {
        var existingTopic = await topicRepository.GetByIdAsync(id);
        if (existingTopic == null)
            return NotFound($"Topic with id {id} not found");
        
        var topic = TopicMapper.ToEntity(existingTopic, request);
        await topicRepository.UpdateAsync(topic);

        var dto = TopicMapper.ToDto(existingTopic);
        return Ok(dto);
    }

    /// <summary>
    /// Удалить тему по ID.
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTopic(int id)
    {
        var existingTopic = await topicRepository.GetByIdAsync(id);
        if (existingTopic == null)
            return NotFound($"Topic with id {id} not found");

        await topicRepository.DeleteAsync(existingTopic);
        return NoContent();
    }

}