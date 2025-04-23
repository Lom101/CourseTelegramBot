using Backend.Dto.Block.Request;
using Backend.Mapper;
using Core.Entity;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BlockController(IBlockRepository blockRepository) : ControllerBase
{ 
    [HttpGet("{id}")]
    public async Task<IActionResult> GetBlockById(int id)
    {
        var block = await blockRepository.GetByIdAsync(id);
        if (block == null)
            return NotFound($"Block with id {id} not found");

        var dto = BlockMapper.ToDto(block);
        return Ok(dto);
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAllBlocks()
    {
        var blocks = await blockRepository.GetAllAsync();
        var dtos = blocks.Select(BlockMapper.ToDto);
        return Ok(dtos);
    }
    
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