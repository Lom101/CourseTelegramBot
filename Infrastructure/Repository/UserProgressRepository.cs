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
    
    public async Task<List<int>> GetCompletedTopicIdsAsync(int userId, int blockId)
    {
        // Находим все Topic.Id, которые пользователь завершил в этом блоке
        return await _context.TopicProgresses
            .Where(tp => tp.UserId == userId && tp.IsCompleted && tp.BlockId == blockId)
            .Select(tp => tp.TopicId)
            .ToListAsync();
    } 
    
    public async Task MarkTopicCompletedAsync(int userId, int topicId)
    {
        var progress = await _context.TopicProgresses
            .FirstOrDefaultAsync(tp => tp.UserId == userId && tp.TopicId == topicId);

        // Получаем блок для темы
        var topic = await _context.Topics.Include(t => t.Block).FirstOrDefaultAsync(t => t.Id == topicId);

        // Если тема не найдена, выбрасываем исключение
        if (topic == null)
        {
            throw new InvalidOperationException("Topic not found.");
        }
        
        if (progress == null)
        {
            // Создаем новый прогресс для темы
            progress = new TopicProgress
            {
                UserId = userId,
                TopicId = topicId,
                BlockId = topic.BlockId,  // Заполняем BlockId, полученный от темы
                IsCompleted = true,
                UpdatedAt = DateTime.UtcNow
            };
        
            _context.TopicProgresses.Add(progress);
        }
        else
        {
            // Обновляем существующий прогресс
            progress.IsCompleted = true;
            progress.UpdatedAt = DateTime.UtcNow;
            progress.BlockId = topic.BlockId;  // Заполняем BlockId при обновлении
            _context.TopicProgresses.Update(progress);
        }

        // Сохраняем изменения
        await _context.SaveChangesAsync();
    }
    
    // Получение прогресса по теме
    public async Task<TopicProgress?> GetTopicProgressAsync(int userId, int topicId)
    {
        return await _context.TopicProgresses
            .FirstOrDefaultAsync(x => x.UserId == userId && x.TopicId == topicId);
    }

    // Получение прогресса по финальному тесту
    public async Task<FinalTestProgress?> GetFinalTestProgressAsync(int userId, int blockId)
    {
        return await _context.FinalTestProgresses
            .FirstOrDefaultAsync(x => x.UserId == userId && x.BlockId == blockId);
    }

    // Получение прогресса по завершению курса
    public async Task<BlockCompletionProgress?> GetBlockCompletionProgressAsync(int userId, int blockId)
    {
        return await _context.BlockCompletionProgresses
            .FirstOrDefaultAsync(x => x.UserId == userId && x.BlockId == blockId);
    }

    // Сохранение прогресса по теме
    public async Task SaveTopicProgressAsync(TopicProgress progress)
    {
        _context.TopicProgresses.Update(progress);
        await _context.SaveChangesAsync();
    }

    // Сохранение прогресса по финальному тесту
    public async Task SaveFinalTestProgressAsync(FinalTestProgress progress)
    {
        _context.FinalTestProgresses.Update(progress);
        await _context.SaveChangesAsync();
    }

    // Сохранение прогресса по завершению курса
    public async Task SaveCourseCompletionProgressAsync(BlockCompletionProgress progress)
    {
        _context.BlockCompletionProgresses.Update(progress);
        await _context.SaveChangesAsync();
    }
}