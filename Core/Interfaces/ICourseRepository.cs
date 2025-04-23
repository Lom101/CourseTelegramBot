using Core.Entity;

namespace Core.Interfaces;

public interface ICourseRepository
{
    Task<Course?> GetByIdAsync(int id);
    Task<List<Course>> GetAllAsync();
    Task<Course?> GetByIdWithTopicsAsync(int id);
    Task AddAsync(Course course);
    Task UpdateAsync(Course course);
    Task DeleteAsync(Course course);
}