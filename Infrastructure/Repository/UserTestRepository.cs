using Core.Entity.Test;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository;

public class UserTestRepository : IUserTestRepository
{
    private readonly AppDbContext _context;

    public UserTestRepository(AppDbContext context)
    {
        _context = context;
    }
    
    public async Task<UserTest?> GetByChatIdAndTestIdAsync(long chatId, int testId)
    {
        return await _context.UserTests
            .FirstOrDefaultAsync(ut => ut.ChatId == chatId && ut.TestId == testId);
    }


    public async Task<UserTest?> GetByChatIdAsync(long chatId)
    {
        return await _context.UserTests
            .Include(ut => ut.Test)
            .ThenInclude(t => t.Questions)
            .ThenInclude(q => q.Options)
            .FirstOrDefaultAsync(ut => ut.ChatId == chatId);
    }

    public async Task AddAsync(UserTest userTest)
    {
        await _context.UserTests.AddAsync(userTest);
        await _context.SaveChangesAsync();
    }

    public async Task SaveAnswerAsync(UserTestAnswer answer)
    {
        await _context.UserTestAnswers.AddAsync(answer);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(UserTest userTest)
    {
        _context.UserTests.Update(userTest);
        await _context.SaveChangesAsync();
    }
}
