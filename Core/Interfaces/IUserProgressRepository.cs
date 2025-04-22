using Core.Entity;

namespace Core.Interfaces;

public interface IUserProgressRepository
{
    Task<UserProgress?> GetByIdAsync(int id);
    Task<UserProgress?> GetByUserAndContentAsync(int userId, int contentId);
    Task<List<UserProgress>> GetByUserIdAsync(int userId);
    Task AddAsync(UserProgress progress);
    Task UpdateAsync(UserProgress progress);
}   