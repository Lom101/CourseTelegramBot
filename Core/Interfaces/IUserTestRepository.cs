using Core.Entity.Test;

namespace Core.Interfaces;

public interface IUserTestRepository
{
    Task<UserTest?> GetByChatIdAsync(long chatId);
    Task AddAsync(UserTest userTest);
    Task SaveAnswerAsync(UserTestAnswer answer);
    Task UpdateAsync(UserTest userTest);
}
