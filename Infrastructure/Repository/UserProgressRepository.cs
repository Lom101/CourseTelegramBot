using Core.Entity;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository;

public class UserProgressRepository : IUserProgressRepository
{
    private readonly AppDbContext _context;

    public UserProgressRepository(AppDbContext context)
    {
        _context = context;
    }
    
    public async Task<UserProgress?> GetByIdAsync(int id)
    {
        return await _context.UserProgresses.FindAsync(id);
    }

    public async Task<UserProgress?> GetByUserAndContentAsync(int userId, int contentId)
    {
        return await _context.UserProgresses
            .FirstOrDefaultAsync(up => up.UserId == userId && up.ContentId == contentId);
    }

    public async Task<List<UserProgress>> GetByUserIdAsync(int userId)
    {
        return await _context.UserProgresses
            .Where(up => up.UserId == userId)
            .ToListAsync();
    }

    public async Task AddAsync(UserProgress progress)
    {
        await _context.UserProgresses.AddAsync(progress);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(UserProgress progress)
    {
        _context.UserProgresses.Update(progress);
        await _context.SaveChangesAsync();
    }
}