using Core.Entity;

namespace Core.Interfaces;

public interface IContentItemRepository
{
    Task<ContentItem> GetByIdAsync(long id);
    Task<IEnumerable<ContentItem>> GetByTopicIdAsync(long topicId);
    Task AddAsync(ContentItem contentItem);
    Task UpdateAsync(ContentItem contentItem);
    Task DeleteAsync(ContentItem contentItem);
}