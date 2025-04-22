using Core.Entity;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository;

public class ContentItemRepository : IContentItemRepository
{
    private readonly AppDbContext _context;

    public ContentItemRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<ContentItem> GetByIdAsync(long id)
    {
        return await _context.ContentItems.
            FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<IEnumerable<ContentItem>> GetByTopicIdAsync(long topicId)
    {
        return await _context.ContentItems
            .Where(c => c.TopicId == topicId)
            .OrderBy(c => c.Order) // Сортируем контент по порядку
            .ToListAsync();
    }

    public async Task AddAsync(ContentItem contentItem)
    {
        await _context.ContentItems.AddAsync(contentItem);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(ContentItem contentItem)
    {
        _context.ContentItems.Update(contentItem);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(ContentItem contentItem)
    {
        _context.ContentItems.Remove(contentItem);
        await _context.SaveChangesAsync();
    }
}