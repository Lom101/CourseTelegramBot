using Core.Entity;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository;

public class BlockRepository : IBlockRepository
{
    private readonly AppDbContext _context;

    public BlockRepository(AppDbContext context)
    {
        _context = context;
    }
    
    public async Task<Block?> GetByIdAsync(int id)
    {
        return await _context.Blocks.FindAsync(id);
    }

    public async Task<List<Block>> GetAllAsync()
    {
        return await _context.Blocks.ToListAsync();
    }

    public async Task<Block?> GetByIdWithTopicsAsync(int id)
    {
        return await _context.Blocks
            .Include(c => c.Topics)
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task AddAsync(Block block)
    {
        await _context.Blocks.AddAsync(block);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Block block)
    {
        _context.Blocks.Update(block);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Block block)
    {
        _context.Blocks.Remove(block);
        await _context.SaveChangesAsync();
    }
}