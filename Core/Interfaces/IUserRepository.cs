using Core.Entity;
using Core.Model;

namespace Core.Interfaces;

public interface IUserRepository
{
    Task<User?> GetByIdAsync(int id);
    Task<User?> GetByChatIdAsync(long chatId);
    Task<User?> GetByPhoneNumberAsync(string phoneNumber);
    Task<User?> GetByEmailAsync(string email);
    Task<List<User>> GetAllAsync();
    Task<IEnumerable<User>> GetFilteredUsersAsync(UserFilterModel filterRequest);
    Task AddAsync(User user);
    Task UpdateAsync(User user);
    Task DeleteAsync(User user);
    Task<bool> IsAuthorizedAsync(long chatId);
}