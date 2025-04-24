using Core.Entity.Test;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

public class TestRepository : ITestRepository
{
    private readonly AppDbContext _context;

    public TestRepository(AppDbContext context)
    {
        _context = context;
    }
    
    public async Task<Core.Entity.Test.Test?> GetByTopicIdAsync(int topicId)
    {
        return await _context.Topics
            .Where(t => t.Id == topicId)  // Фильтруем по topicId
            .Include(t => t.Block)         // Включаем Block для доступа к Test
            .ThenInclude(b => b.Test)      // Включаем Test, чтобы его получить
            .ThenInclude(t => t.Questions) // Включаем Questions для Test
            .ThenInclude(q => q.Options)  // Включаем Options для Questions
            .Select(t => t.Block.Test)    // Проецируем на Test, так как Block.Test - это нужная сущность
            .FirstOrDefaultAsync();       // Получаем первый результат или null
    }

    public async Task<Test?> GetByBlockIdAsync(int blockId)
    {
        return await _context.Blocks
            .Where(b => b.Id == blockId)
            .Include(b => b.Test)  // Загрузка связанного теста через Block
            .ThenInclude(t => t.Questions)  // Включение вопросов теста
            .ThenInclude(q => q.Options)  // Включение вариантов ответов на вопросы
            .Select(b => b.Test)
            .FirstOrDefaultAsync();
    }
    
    public async Task<Test?> GetByIdAsync(int testId)
    {
        return await _context.Tests
            .Include(t => t.Questions)
            .ThenInclude(q => q.Options)
            .FirstOrDefaultAsync(t => t.Id == testId);
    }


}