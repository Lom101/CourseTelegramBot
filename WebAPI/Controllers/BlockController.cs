using Backend.Dto.Block.Request;
using Backend.Mapper;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

/// <summary>
/// Контроллер для управления главами курса.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class BlockController(IBlockRepository blockRepository) : ControllerBase
{ 
    /// <summary>
    /// Получить главу по ID.
    /// </summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetBlockById(int id)
    {
        var block = await blockRepository.GetByIdAsync(id);
        if (block == null)
            return NotFound($"Block with id {id} not found");

        var dto = BlockMapper.ToDto(block);
        return Ok(dto);
    }
    
    [HttpGet("with-topics/{id}")]
    public async Task<IActionResult> GetByIdWithTopicsAsync(int id)
    {
        var block = await blockRepository.GetByIdAsync(id);
        if (block == null)
            return NotFound($"Block with id {id} not found");

        var dto = BlockMapper.ToDto(block);
        return Ok(dto);
    }
    
    /// <summary>
    /// Получить список всех глав.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAllBlocks()
    {
        var blocks = await blockRepository.GetAllAsync();
        var dtos = blocks.Select(BlockMapper.ToDto);
        return Ok(dtos);
    }
    
    
    /// <summary>
    /// Создать новую главу.
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> CreateBlock([FromBody] CreateBlockRequest request)
    {
        if (request == null)
            return BadRequest("Block data is required");
        
        var block = BlockMapper.ToEntity(request);
        await blockRepository.AddAsync(block);
        var dto = BlockMapper.ToDto(block);
        return CreatedAtAction(nameof(GetBlockById), new { id = dto.Id }, dto);

    }
    
    /// <summary>
    /// Обновить существующую главу.
    /// </summary>
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateBlock(int id, [FromBody] UpdateBlockRequest request)
    {
        var existingBlock = await blockRepository.GetByIdAsync(id);
        if (existingBlock == null)
            return NotFound($"Block with id {id} not found");

        BlockMapper.ToEntity(existingBlock, request);
        await blockRepository.UpdateAsync(existingBlock);

        var dto = BlockMapper.ToDto(existingBlock);
        return Ok(dto);
    }
        
    /// <summary>
    /// Удалить главу по ID.
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteBlock(int id)
    {
        var block = await blockRepository.GetByIdAsync(id);
        if (block == null)
            return NotFound($"Block with id {id} not found");

        await blockRepository.DeleteAsync(block);
        return NoContent();
    }
}