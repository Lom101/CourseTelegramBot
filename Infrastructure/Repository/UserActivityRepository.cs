using Core.Entity;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository;

public class UserActivityRepository : IUserActivityRepository
{
    private readonly AppDbContext _context;

    public UserActivityRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<UserActivity?> GetByIdAsync(int id)
    {
        return await _context.UserActivities.FindAsync(id);
    }

    public async Task<List<UserActivity>> GetByUserIdAsync(int userId)
    {
        return await _context.UserActivities
            .Where(ua => ua.UserId == userId)
            .OrderBy(ua => ua.Id)
            .ToListAsync();
    }
    
    public async Task AddAsync(UserActivity activity)
    {
        await _context.UserActivities.AddAsync(activity);
        await _context.SaveChangesAsync();
    }
}