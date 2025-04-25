using Core.Entity;

namespace Core.Interfaces;

public interface ITopicRepository
{
    Task<Topic?> GetByIdAsync(int id);
    Task<List<Topic>> GetByBlockIdAsync(int courseId);
    Task AddAsync(Topic topic);
    Task UpdateAsync(Topic topic);
    Task DeleteAsync(Topic topic);
}