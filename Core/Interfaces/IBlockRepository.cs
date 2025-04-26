using Core.Entity;

namespace Core.Interfaces;

public interface IBlockRepository
{
    Task<Block?> GetByIdAsync(int id);
    Task<List<Block>> GetAllAsync();
    Task<Block?> GetByIdWithTopicsAsync(int id);
    Task AddAsync(Block block);
    Task UpdateAsync(Block block);
    Task DeleteAsync(Block block);
    Task<Block> GetByTestIdAsync(int testId);
}