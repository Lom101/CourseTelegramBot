using Core.Entity;

namespace Core.Interfaces;

public interface IUserProgressRepository
{
    Task<UserProgress?> GetByIdAsync(int id);
    Task<List<UserProgress>> GetByUserIdAsync(int userId);
    Task AddAsync(UserProgress progress);
}   