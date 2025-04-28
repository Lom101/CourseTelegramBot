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
                PassedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                CorrectAnswersCount = correctAnswersCount
            };

            _context.FinalTestProgresses.Add(existingProgress);
        }
        else
        {
            existingProgress.IsPassed = true;
            existingProgress.PassedAt = DateTime.UtcNow;
            existingProgress.UpdatedAt = DateTime.UtcNow;
            existingProgress.CorrectAnswersCount = correctAnswersCount; 
        }

        await _context.SaveChangesAsync();
    }
    
    public async Task<bool> IsTestCompletedAsync(long chatId, int blockId)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.ChatId == chatId);
        
        // Проверка, был ли завершен тест для этого пользователя и блока
        return await _context.FinalTestProgresses
            .AnyAsync(up => up.UserId == user.Id && up.BlockId == blockId && up.IsPassed);
    }
    
    
    
    public Task MarkBlockAsCompletedIfAllTopicsCompleted(int userId, int blockId)
    {
        throw new NotImplementedException();
    }
    
    // Завершен ли блок? Пройдены ли все темы в блоке
    public async Task<BlockCompletionProgress?> GetBlockCompletionProgressAsync(int userId, int blockId)
    {
        return await _context.BlockCompletionProgresses
            .FirstOrDefaultAsync(x => x.UserId == userId && x.BlockId == blockId);
    }
    
    public async Task MarkBlockAsCompletedAsync(int userId, int blockId)
    {
        var progress = await _context.BlockCompletionProgresses
            .FirstOrDefaultAsync(x => x.UserId == userId && x.BlockId == blockId);

        if (progress == null)
        {
            // Если прогресс для блока ещё не существует, создаём новый
            progress = new BlockCompletionProgress
            {
                UserId = userId,
                BlockId = blockId,
                IsBlockCompleted = true
            };
        
            _context.BlockCompletionProgresses.Add(progress);
        }
        else
        {
            // Обновляем существующий прогресс
            progress.IsBlockCompleted = true;
        }
    
        await _context.SaveChangesAsync();
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

    // Task MarkBlockAsCompletedIfAllTopicsCompleted(int userId, int blockId){
    //     // Получаем все темы блока
    //     var topics = await _topicRepository.GetByBlockIdAsync(blockId);
    //
    //     // Проверяем, все ли темы пройдены
    //     var allTopicsCompleted = true;
    //     foreach (var topic in topics)
    //     {
    //         var topicProgress = await _userProgressRepository.GetTopicProgressAsync(userId, topic.Id);
    //         if (topicProgress == null || topicProgress.IsCompleted != true)
    //         {
    //             allTopicsCompleted = false;
    //             break;
    //         }
    //     }
    //
    //     if (allTopicsCompleted)
    //     {
    //         // Если все темы пройдены, обновляем блок как завершённый
    //         await _userProgressRepository.MarkBlockAsCompletedAsync(userId, blockId);
    //     }
    // }
    
    
    public async Task<List<int>> GetCompletedTopicIdsAsync(int userId, int blockId)
    {
        // Находим все Topic.Id, которые пользователь завершил в этом блоке
        return await _context.TopicProgresses
            .Where(tp => tp.UserId == userId && tp.IsCompleted && tp.BlockId == blockId)
            .OrderBy(tp => tp.TopicId) // Сортировка по TopicId
            .Select(tp => tp.TopicId)
            .ToListAsync();
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

}