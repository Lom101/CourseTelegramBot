using Core.Entity.Test;

namespace Core.Interfaces;

public interface ITestRepository
{
    Task<Test?> GetByBlockIdAsync(int blockId);
    Task<Test?> GetByIdAsync(int testId);
    Task<Core.Entity.Test.Test?> GetByTopicIdAsync(int topicId);
}