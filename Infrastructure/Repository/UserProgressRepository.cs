using Core.Entity;
using Core.Entity.Progress;
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
    
    
    // Получение прогресса по теме
    public async Task SaveFinalTestResultAsync(long chatId, int finalTestId, int correctAnswersCount)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.ChatId == chatId);

        if (user == null)
            return;

        var block = await _context.Blocks
            .Include(b => b.UserProgress)
            .FirstOrDefaultAsync(b => b.FinalTestId == finalTestId);

        if (block == null)
            return;

        var existingProgress = await _context.FinalTestProgresses
            .FirstOrDefaultAsync(p => p.UserId == user.Id && p.BlockId == block.Id);
        
        if (existingProgress == null)
        {
            existingProgress = new FinalTestProgress()
            {
                UserId = user.Id,
                BlockId = block.Id,
                IsPassed = true,
                PassedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                CorrectAnswersCount = correctAnswersCount
            };

            _context.FinalTestProgresses.Add(existingProgress);
        }
        else
        {
            existingProgress.IsPassed = true;
            existingProgress.PassedAt = DateTime.Now;
            existingProgress.UpdatedAt = DateTime.Now;
            existingProgress.CorrectAnswersCount = correctAnswersCount; 
        }

        await _context.SaveChangesAsync();
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
}