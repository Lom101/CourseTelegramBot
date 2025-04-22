using Core.Entity;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository;

public class TopicRepository : ITopicRepository
{
    private readonly AppDbContext _context;
    private ITopicRepository _topicRepositoryImplementation;

    public TopicRepository(AppDbContext context)
    {
        _context = context;
    }
    
    public async Task<Topic?> GetByIdAsync(int id)
    {
        return await _context.Topics
            .Include(t => t.ContentItems)
            .FirstOrDefaultAsync(t => t.Id == id);
    }

    public async Task<List<Topic>> GetByCourseIdAsync(int courseId)
    {
        return await _context.Topics
            .Where(t => t.CourseId == courseId)
            .ToListAsync();
    }

    public async Task<Topic?> GetWithContentItemsAsync(int topicId)
    {
        return await _context.Topics
            .Include(t => t.ContentItems)
            .FirstOrDefaultAsync(t => t.Id == topicId);
    }

    public async Task AddAsync(Topic topic)
    {
        await _context.Topics.AddAsync(topic);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Topic topic)
    {
        _context.Topics.Update(topic);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Topic topic)
    {
        _context.Topics.Remove(topic);
        await _context.SaveChangesAsync();
    }
    
    
    
}