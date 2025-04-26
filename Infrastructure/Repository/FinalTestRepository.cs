using Core.Entity.Test;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

public class FinalTestRepository : IFinalTestRepository
{
    private readonly AppDbContext _context;

    public FinalTestRepository(AppDbContext context)
    {
        _context = context;
    }
    
    public async Task<int> CreateAsync(FinalTest finalTest)
    {
        _context.FinalTests.Add(finalTest);
        await _context.SaveChangesAsync();
        return finalTest.Id;
    }
    
    public async Task<List<FinalTest>> GetAllAsync()
    {
        return await _context.FinalTests
            .Include(f => f.Questions)
            .ThenInclude(q => q.Options)
            .ToListAsync();
    }
    
    public async Task<FinalTest> GetByBlockIdAsync(int blockId)
    {
        return await _context.Blocks
            .Where(b => b.Id == blockId)
            .Include(b => b.FinalTest)  // Загрузка связанного теста через Block
            .ThenInclude(t => t.Questions)  // Включение вопросов теста
            .ThenInclude(q => q.Options)  // Включение вариантов ответов на вопросы
            .Select(b => b.FinalTest)
            .FirstOrDefaultAsync();
    }
    
    public async Task<FinalTest?> GetByIdAsync(int testId)
    {
        var test = await _context.FinalTests
            .Include(t => t.Questions)
            .ThenInclude(q => q.Options)
            .FirstOrDefaultAsync(t => t.Id == testId);

        if (test != null)
        {
            test.Questions = test.Questions.OrderBy(q => q.Id).ToList(); // здесь сортировка
        }

        return test;
    }
}