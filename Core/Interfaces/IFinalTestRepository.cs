using Core.Entity.Test;

namespace Core.Interfaces;

public interface IFinalTestRepository
{
    Task<FinalTest> GetByBlockIdAsync(int blockId);
    Task<FinalTest?> GetByIdAsync(int testId);
    Task<FinalTest> GetByTopicIdAsync(int topicId);
}