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
        return topics.Any() ? Ok(topics) : throw new KeyNotFoundException($"No topics found for course {courseId}");
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetTopicById(int id)
    {
        var topic = await topicRepository.GetByIdAsync(id);
        if (topic == null)
            throw new KeyNotFoundException($"Topic with id {id} not found");

        return Ok(topic);
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateTopic([FromBody] Topic topic)
    {
        await topicRepository.AddAsync(topic);
        return CreatedAtAction(nameof(GetTopicById), new { id = topic.Id }, topic);
    }
}