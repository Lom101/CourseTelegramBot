using Core.Entity;

namespace Core.Interfaces;

public interface ITopicRepository
{
    Task<Topic?> GetByIdAsync(int id);
    Task<List<Topic>> GetByCourseIdAsync(int courseId);
    Task<Topic?> GetWithContentItemsAsync(int topicId);
    Task AddAsync(Topic topic);
    Task UpdateAsync(Topic topic);
    Task DeleteAsync(Topic topic);
}