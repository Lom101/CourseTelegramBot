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
    /// Контроллер для управления контентом внутри топиков.
    /// Поддерживает создание, получение, обновление порядка и удаление контент-элементов:
    /// текст, изображение и ссылка.
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
        private async Task<int> CalculateOrderAsync(int topicId)
        {
            var contentItems = await contentItemRepository.GetByTopicIdAsync(topicId);
            return contentItems.Any() ? contentItems.Max(c => c.Order) + 1 : 1;
        }
        
        // Метод для получения контента по ID с маппингом
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
        /// Получить список всех контент-элементов, принадлежащих указанному топику.
        /// </summary>
        /// <param name="topicId">Уникальный идентификатор топика.</param>
        /// <returns>
        /// Возвращает отсортированный список <see cref="ContentItem"/> по полю Order,
        /// либо код 404, если топик или его контент отсутствует.
        /// </returns>
        [HttpGet("by-topic/{topicId}")]
        public async Task<IActionResult> GetAllContentByTopicId(int topicId)
        {
            var topic = await topicRepository.GetByIdAsync(topicId);
            if (topic == null)
                return NotFound($"Topic with id {topicId} not found");
            
            var contentItems = await contentItemRepository.GetByTopicIdAsync(topicId);
            var sortedContentItems = contentItems.OrderBy(c => c.Order).ToList(); // Сортируем по порядку
            
            var dtoList = sortedContentItems.Select(ContentItemMappingService.MapToDto).ToList(); // маппим
            
            return sortedContentItems.Any() ? Ok(dtoList) : NotFound($"No content found for topic with id {topicId}");
        }

        [HttpPost("word")]
        public async Task<IActionResult> CreateWordFileContent([FromForm] CreateWordFileContentRequest request)
        {
            if (request.File == null || request.File.Length == 0)
                return BadRequest("Файл обязателен");

            var topic = await topicRepository.GetByIdAsync(request.TopicId);
            if (topic == null)
                return NotFound($"Топик с ID {request.TopicId} не найден");

            var fileUrl = await wordFileService.SaveWordFileAsync(request.File);
            var order = await CalculateOrderAsync(request.TopicId);

            var content = ContentItemMapper.ToEntity(request, fileUrl, order);

            await contentItemRepository.AddAsync(content);
            
            var dto = ContentItemMappingService.MapToDto(content);
            return CreatedAtAction(nameof(GetContentById), new { id = content.Id }, dto);
        }

        /// <summary>
        /// Создать изображение как контент внутри топика.
        /// </summary>
        /// <param name="request">Объект запроса с ID топика и файлом изображения.</param>
        /// <returns>
        /// Возвращает созданный объект <see cref="ImageContent"/> с кодом 201,
        /// либо код 404, если топик не найден.
        /// </returns>
        [HttpPost("image")]
        public async Task<IActionResult> CreateImageContent([FromForm] CreateImageContentRequest request)
        {
            var topic = await topicRepository.GetByIdAsync(request.TopicId);
            if (topic == null)
                return NotFound($"Topic with id {request.TopicId} not found");
            
            var imageUrl = await imageFileService.SaveImageAsync(request.Image);
            var order = await CalculateOrderAsync(request.TopicId);

            var content = ContentItemMapper.ToEntity(request, imageUrl, order);
            
            await contentItemRepository.AddAsync(content);

            var dto = ContentItemMappingService.MapToDto(content);
            return CreatedAtAction(nameof(GetContentById), new { id = content.Id }, dto);
        }
        
        /// <summary>
        /// Создать книжный файл как контент внутри топика.
        /// </summary>
        /// <param name="request">Объект запроса с ID топика и файлом книги (PDF, EPUB и т.п.).</param>
        /// <returns>
        /// Возвращает созданный объект <see cref="BookContent"/> с кодом 201,
        /// либо код 404, если топик не найден.
        /// </returns>
        [HttpPost("book")]
        public async Task<IActionResult> CreateBookContent([FromForm] CreateBookContentRequest request)
        {
            var topic = await topicRepository.GetByIdAsync(request.TopicId);
            if (topic == null)
                return NotFound($"Topic with id {request.TopicId} not found");

            var fileUrl = await bookFileService.SaveBookAsync(request.File);
            var order = await CalculateOrderAsync(request.TopicId);

            var content = ContentItemMapper.ToEntity(request, fileUrl, order);
            
            await contentItemRepository.AddAsync(content);
            
            var dto = ContentItemMappingService.MapToDto(content);
            return CreatedAtAction(nameof(GetContentById), new { id = content.Id }, dto);
        }
        
        /// <summary>
        /// Создать аудиофайл как контент внутри топика.
        /// </summary>
        /// <param name="request">Объект запроса с ID топика, аудиофайлом и опциональным названием аудио.</param>
        /// <returns>
        /// Возвращает созданный объект <see cref="AudioContent"/> с кодом 201,
        /// либо код 404, если топик не найден.
        /// </returns>
        [HttpPost("audio")]
        public async Task<IActionResult> CreateAudioContent([FromForm] CreateAudioContentRequest request)
        {
            var topic = await topicRepository.GetByIdAsync(request.TopicId);
            if (topic == null)
                return NotFound($"Topic with id {request.TopicId} not found");

            var audioUrl = await audioFileService.SaveAudioAsync(request.AudioFile); 
            var order = await CalculateOrderAsync(request.TopicId);

            var content = ContentItemMapper.ToEntity(request, audioUrl, order);
            
            await contentItemRepository.AddAsync(content);

            var dto = ContentItemMappingService.MapToDto(content);
            return CreatedAtAction(nameof(GetContentById), new { id = content.Id }, dto);
        }

        
        /// <summary>
        /// Обновить порядок (позицию) контент-элемента внутри своего топика.
        /// </summary>
        /// <param name="id">Уникальный идентификатор контент-элемента.</param>
        /// <param name="newOrder">Новый порядковый номер элемента.</param>
        /// <returns>
        /// Возвращает обновлённый объект <see cref="ContentItem"/> с кодом 200,
        /// либо код 400/404 при ошибках.
        /// </returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateContentOrder(int id, [FromBody] int newOrder)
        {
            var contentItem = await contentItemRepository.GetByIdAsync(id);
            if (contentItem == null)
                return NotFound($"Content item with id {id} not found");

            var allContent = await contentItemRepository.GetByTopicIdAsync(contentItem.TopicId);

            if (allContent == null || !allContent.Any())
                return BadRequest("Content list is empty or not initialized");

            // Проверяем, что новый порядок валиден
            if (newOrder < 1 || newOrder > allContent.Count())
                return BadRequest("Invalid order number");
            
            // Обновляем порядок
            contentItem.Order = newOrder;
            await contentItemRepository.UpdateAsync(contentItem);

            var dto = ContentItemMappingService.MapToDto(contentItem);
            return Ok(dto);
        }

        /// <summary>
        /// Удалить контент-элемент по его идентификатору.
        /// </summary>
        /// <param name="id">Уникальный идентификатор контент-элемента.</param>
        /// <returns>
        /// Возвращает код 204 при успешном удалении, либо код 404, если элемент не найден.
        /// </returns>
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
