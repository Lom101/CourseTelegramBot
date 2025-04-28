using Core.Entity;
using Core.Interfaces;
using Core.Model;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;

    public UserRepository(AppDbContext context)
    {
        _context = context;
    }
    
    public async Task UpdateLastActivityAsync(long chatId)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.ChatId == chatId);

        if (user != null)
        {
            user.LastActivity = DateTime.UtcNow;
            await _context.SaveChangesAsync();
        }
    }
    
    public async Task<IEnumerable<User>> GetFilteredUsersAsync(UserFilterModel filterRequest)
    {
        IQueryable<User> query = _context.Users;

        if (!string.IsNullOrEmpty(filterRequest.FullName))
            query = query.Where(u => u.FullName.Contains(filterRequest.FullName));

        // TODO: сделать обработку CompletedMaterialCount
        // if (filterRequest.CompletedMaterialCount.HasValue)
        // {
        //     query = query.Where(u =>
        //         _context.UserProgresses
        //             .Where(p => p.UserId == u.Id && p.)
        //             .Count() >= filterRequest.CompletedMaterialCount.Value);
        // }


        if (filterRequest.RegistrationDateFrom.HasValue)
            query = query.Where(u => u.RegistrationDate >= filterRequest.RegistrationDateFrom.Value);

        if (filterRequest.RegistrationDateTo.HasValue)
            query = query.Where(u => u.RegistrationDate <= filterRequest.RegistrationDateTo.Value);

        if (filterRequest.IsBlocked.HasValue)
            query = query.Where(u => u.IsBlocked == filterRequest.IsBlocked.Value);

        if (filterRequest.IsAdmin.HasValue)
            query = query.Where(u => u.IsAdmin == filterRequest.IsAdmin.Value);

        return await query.ToListAsync();
    }
    
    public async Task<User?> GetByIdAsync(int id)
    {
        return await _context.Users.FindAsync(id);
    }

    public async Task<User?> GetByChatIdAsync(long chatId)
    {
        return await _context.Users
            .FirstOrDefaultAsync(u => u.ChatId == chatId);
    }

    public async Task<User?> GetByPhoneNumberAsync(string phoneNumber)
    {
        return await _context.Users
            .FirstOrDefaultAsync(u => u.PhoneNumber == phoneNumber);
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        return await _context.Users
            .FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<List<User>> GetAllAsync()
    {
        return await _context.Users.ToListAsync();
    }

    public async Task AddAsync(User user)
    {
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
    }
    
    public async Task UpdateAsync(User user)
    {
        _context.Users.Update(user);
        await _context.SaveChangesAsync();
    }
    
    public async Task DeleteAsync(User user)
    {
        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> IsAuthorizedAsync(long chatId)
    {
        return await _context.Users.AnyAsync(u => u.ChatId == chatId);
    }
}