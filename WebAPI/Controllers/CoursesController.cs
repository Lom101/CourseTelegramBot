using Core.Entity;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CoursesController(ICourseRepository courseRepository) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Course>>> GetAll()
    {
        var courses = await courseRepository.GetAllAsync();
        return Ok(courses);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Course>> GetById(int id)
    {
        var course = await courseRepository.GetByIdAsync(id);
        return course ?? throw new KeyNotFoundException($"Course with id {id} not found");
    }

    [HttpPost]
    public async Task<ActionResult<Course>> Create([FromBody] Course course)
    {
        if (!ModelState.IsValid)
            throw new ArgumentException("Invalid course data");

        await courseRepository.AddAsync(course);
        return CreatedAtAction(nameof(GetById), new { id = course.Id }, course);
    }
}