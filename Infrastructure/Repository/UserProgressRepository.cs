using Core.Entity;
using Core.Entity.Progress;
using Core.Interfaces;
using Core.Model;
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
    
    
    public async Task SaveFinalTestResultAsync(long chatId, int finalTestId, int correctAnswersCount)
    {
        // Получение прогресса по теме //
        
        
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
    
    public async Task<BlockCompletionProgress?> GetBlockCompletionProgressAsync(int userId, int blockId)
    {
        // Завершен ли блок? Пройдены ли все темы в блоке
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

    public async Task<UserProgressDetails> GetUserProgressDetailsAsync(int userId)
    {
        // Получаем все темы, которые пользователь завершил
        var completedTopics = await _context.TopicProgresses
            .Where(tp => tp.UserId == userId && tp.IsCompleted)
            .ToListAsync();
    
        // Получаем все финальные тесты, которые пользователь прошел
        var finalTestProgresses = await _context.FinalTestProgresses
            .Where(ft => ft.UserId == userId && ft.IsPassed)
            .Include(ft => ft.Block)
            .ToListAsync();

        // Получаем прогресс по завершению блоков
        var blockCompletionProgresses = await _context.BlockCompletionProgresses
            .Where(bcp => bcp.UserId == userId)
            .ToListAsync();
    
        // Собираем все данные в сущности
        var progressDetails = new UserProgressDetails
        {
            UserId = userId,
            CompletedTopics = completedTopics,
            FinalTestProgresses = finalTestProgresses,
            BlockCompletionProgresses = blockCompletionProgresses
        };

        return progressDetails;
    }

}

// public Task MarkBlockAsCompletedIfAllTopicsCompleted(int userId, int blockId)
// {
//     throw new NotImplementedException();
// }


// // Получение прогресса по финальному тесту
// public async Task<FinalTestProgress?> GetFinalTestProgressAsync(int userId, int blockId)
// {
//     return await _context.FinalTestProgresses
//         .FirstOrDefaultAsync(x => x.UserId == userId && x.BlockId == blockId);
// }
