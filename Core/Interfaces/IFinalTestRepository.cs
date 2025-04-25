using Core.Entity.Test;

namespace Core.Interfaces;

public interface IFinalTestRepository
{
    Task<int> CreateAsync(FinalTest finalTest);
    Task<List<FinalTest>> GetAllAsync();
    Task<FinalTest> GetByBlockIdAsync(int blockId);
    Task<FinalTest?> GetByIdAsync(int testId);
}