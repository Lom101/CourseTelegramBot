using Core.Entity;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TopicsController(ITopicRepository topicRepository) : ControllerBase
{
    
    
    [HttpGet("by-course/{courseId:int}")]
    public async Task<ActionResult<IEnumerable<Topic>>> GetByCourseId(int courseId)
    {
        var topics = await topicRepository.GetByCourseIdAsync(courseId);
        return topics.Any() ? Ok(topics) : throw new KeyNotFoundException($"No topics found for course {courseId}");
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Topic>> GetById(int id)
    {
        var topic = await topicRepository.GetByIdAsync(id);
        return topic ?? throw new KeyNotFoundException($"Topic with id {id} not found");
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateTopic([FromBody] Topic topic)
    {
        await topicRepository.AddAsync(topic);
        return CreatedAtAction(nameof(GetById), new { id = topic.Id }, topic);
    }
    
}