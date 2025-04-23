using Core.Entity;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backend.Dto;
using Backend.Dto.Audio;
using Backend.Dto.Audio.Response;
using Backend.Dto.Book;
using Backend.Mapper;
using Backend.Mapper.Service;
using Backend.Service;
using Backend.Service.Interfaces;
using Core.Entity.AnyContent;

namespace Backend.Controllers
{
    /// <summary>
    /// Контроллер для управления контентом внутри тем глав курса.
    /// Поддерживает создание, получение и удаление контент-элементов:
    /// word файлы, книги, изображение и аудио.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class ContentItemController(
        IContentItemRepository contentItemRepository,
        ITopicRepository topicRepository, 
        IImageFileService imageFileService,
        IBookFileService bookFileService,
        IAudioFileService audioFileService,
        IWordFileService wordFileService)
        : ControllerBase
    {
        /// <summary>
        /// Получить элемент контента по ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetContentById(int id)
        {
            var contentItem = await contentItemRepository.GetByIdAsync(id);
            if (contentItem == null)
            {
                return NotFound($"Content item with id {id} not found");
            }

            var dto = ContentItemMappingService.MapToDto(contentItem);
            return Ok(dto); 
        }

        /// <summary>
        /// Получить список всех контент-элементов, принадлежащих указанной теме.
        /// </summary>
        [HttpGet("by-topic/{topicId}")]
        public async Task<IActionResult> GetAllContentByTopicId(int topicId)
        {
            var topic = await topicRepository.GetByIdAsync(topicId);
            if (topic == null)
                return NotFound($"Topic with id {topicId} not found");
            
            var contentItems = await contentItemRepository.GetByTopicIdAsync(topicId);
            
            var dtoList = contentItems.Select(ContentItemMappingService.MapToDto).ToList(); // маппим
            
            return dtoList.Any() ? Ok(dtoList) : NotFound($"No content found for topic with id {topicId}");
        }

        /// <summary>
        /// Создать новый word file элемент в выбранной теме
        /// </summary>
        [HttpPost("word")]
        public async Task<IActionResult> CreateWordFileContent([FromForm] CreateWordFileContentRequest request)
        {
            if (request.File == null || request.File.Length == 0)
                return BadRequest("Файл обязателен");

            var topic = await topicRepository.GetByIdAsync(request.TopicId);
            if (topic == null)
                return NotFound($"Топик с ID {request.TopicId} не найден");

            var fileUrl = await wordFileService.SaveWordFileAsync(request.File);

            var content = ContentItemMapper.ToEntity(request, fileUrl);

            await contentItemRepository.AddAsync(content);
            
            var dto = ContentItemMappingService.MapToDto(content);
            return CreatedAtAction(nameof(GetContentById), new { id = content.Id }, dto);
        }

        /// <summary>
        /// Создать новое изображение в выбранной теме
        /// </summary>
        [HttpPost("image")]
        public async Task<IActionResult> CreateImageContent([FromForm] CreateImageContentRequest request)
        {
            var topic = await topicRepository.GetByIdAsync(request.TopicId);
            if (topic == null)
                return NotFound($"Topic with id {request.TopicId} not found");
            
            var imageUrl = await imageFileService.SaveImageAsync(request.Image);

            var content = ContentItemMapper.ToEntity(request, imageUrl);
            
            await contentItemRepository.AddAsync(content);

            var dto = ContentItemMappingService.MapToDto(content);
            return CreatedAtAction(nameof(GetContentById), new { id = content.Id }, dto);
        }
        
        /// <summary>
        /// Создать книжный файл в выбранной теме
        /// </summary>
        [HttpPost("book")]
        public async Task<IActionResult> CreateBookContent([FromForm] CreateBookContentRequest request)
        {
            var topic = await topicRepository.GetByIdAsync(request.TopicId);
            if (topic == null)
                return NotFound($"Topic with id {request.TopicId} not found");

            var fileUrl = await bookFileService.SaveBookAsync(request.File);

            var content = ContentItemMapper.ToEntity(request, fileUrl);
            
            await contentItemRepository.AddAsync(content);
            
            var dto = ContentItemMappingService.MapToDto(content);
            return CreatedAtAction(nameof(GetContentById), new { id = content.Id }, dto);
        }
        
        /// <summary>
        /// Создать аудиофайл в выбранной теме
        /// </summary>
        [HttpPost("audio")]
        public async Task<IActionResult> CreateAudioContent([FromForm] CreateAudioContentRequest request)
        {
            var topic = await topicRepository.GetByIdAsync(request.TopicId);
            if (topic == null)
                return NotFound($"Topic with id {request.TopicId} not found");

            var audioUrl = await audioFileService.SaveAudioAsync(request.AudioFile); 

            var content = ContentItemMapper.ToEntity(request, audioUrl);
            
            await contentItemRepository.AddAsync(content);

            var dto = ContentItemMappingService.MapToDto(content);
            return CreatedAtAction(nameof(GetContentById), new { id = content.Id }, dto);
        }

        
        /// <summary>
        /// Удалить контент-элемент по его id.
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteContent(int id)
        {
            var contentItem = await contentItemRepository.GetByIdAsync(id);
            if (contentItem == null)
                return NotFound($"Content item with id {id} not found");

            await contentItemRepository.DeleteAsync(contentItem);
            return NoContent();
        }
    }
}
