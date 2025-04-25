using Core.Entity;
using Core.Entity.AnyContent;

namespace Core.Interfaces;

public interface IContentItemRepository
{
    Task<ContentItem> GetByIdAsync(long id);
    Task<IEnumerable<ContentItem>> GetByTopicIdAsync(long topicId);
    Task AddAsync(ContentItem contentItem);
    Task DeleteAsync(ContentItem contentItem);
}