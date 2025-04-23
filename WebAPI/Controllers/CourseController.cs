using Core.Entity;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CourseController(ICourseRepository courseRepository) : ControllerBase
{ 
    //[Authorize]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetCourseById(int id = 1)
    {
        var course = await courseRepository.GetByIdAsync(id);
        return Ok(course) ?? throw new KeyNotFoundException($"Course with id {id} not found");
    }
}