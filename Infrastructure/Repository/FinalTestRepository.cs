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
    
    // TODO: переписать запросы
    
    public async Task<FinalTest> GetByTopicIdAsync(int topicId)
    {
        return await _context.Topics
            .Where(t => t.Id == topicId)  // Фильтруем по topicId
            .Include(t => t.Block)         // Включаем Block для доступа к Test
            .ThenInclude(b => b.FinalTest)      // Включаем Test, чтобы его получить
            .ThenInclude(t => t.Questions) // Включаем Questions для Test
            .ThenInclude(q => q.Options)  // Включаем Options для Questions
            .Select(t => t.Block.FinalTest)    // Проецируем на Test, так как Block.Test - это нужная сущность
            .FirstOrDefaultAsync();       // Получаем первый результат или null
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
            test.Questions = test.Questions.OrderBy(q => q.Id).ToList(); // 🧠 здесь сортировка
        }

        return test;
    }
}