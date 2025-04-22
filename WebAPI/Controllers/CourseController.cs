using Core.Entity;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CourseController(ICourseRepository courseRepository) : ControllerBase
{
    [Authorize]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Course>>> GetAllCourses()
    {
        var courses = await courseRepository.GetAllAsync();
        return Ok(courses);
    }

    [Authorize]
    [HttpGet("{id}")]
    public async Task<ActionResult<Course>> GetCourseById(int id)
    {
        var course = await courseRepository.GetByIdAsync(id);
        return course ?? throw new KeyNotFoundException($"Course with id {id} not found");
    }
}