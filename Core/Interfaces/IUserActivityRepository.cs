using Core.Entity;

namespace Core.Interfaces;

public interface IUserActivityRepository
{
    Task<UserActivity?> GetByIdAsync(int id);
    Task<List<UserActivity>> GetByUserIdAsync(int userId);
    Task AddAsync(UserActivity activity);
}