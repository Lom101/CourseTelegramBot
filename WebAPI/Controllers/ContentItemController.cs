using Core.Entity;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backend.Dto;
using Backend.Service;
using Backend.Service.Interfaces;
using Core.Dto.Content.Request;

namespace Backend.Controllers
{
    /// <summary>
    /// Контроллер для управления контентом внутри топиков.
    /// Поддерживает создание, получение, обновление порядка и удаление контент-элементов:
    /// текст, изображение и ссылка.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class ContentItemController(IContentItemRepository contentItemRepository, ITopicRepository topicRepository, IImageFileService imageFileService)
        : ControllerBase
    {
        private async Task<int> CalculateOrderAsync(int topicId)
        {
            var contentItems = await contentItemRepository.GetByTopicIdAsync(topicId);
            return contentItems.Any() ? contentItems.Max(c => c.Order) + 1 : 1;
        }
        
        /// <summary>
        /// Получить контент-элемент по его уникальному идентификатору.
        /// </summary>
        /// <param name="id">Уникальный идентификатор контент-элемента.</param>
        /// <returns>
        /// Возвращает <see cref="ContentItem"/> в случае успеха или код 404, если элемент не найден.
        /// </returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetContentById(int id)
        {
            var contentItem = await contentItemRepository.GetByIdAsync(id);
            return contentItem != null ? Ok(contentItem) : NotFound($"Content item with id {id} not found");
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
            return sortedContentItems.Any() ? Ok(sortedContentItems) : NotFound($"No content found for topic with id {topicId}");
        }

        /// <summary>
        /// Создать текстовый контент внутри топика.
        /// </summary>
        /// <param name="request">Объект запроса, содержащий текст и ID топика.</param>
        /// <returns>
        /// Возвращает созданный объект <see cref="TextContent"/> с кодом 201,
        /// либо код 400/404 при ошибке валидации или отсутствии топика.
        /// </returns>
        [HttpPost("text")]
        public async Task<IActionResult> CreateTextContent([FromBody] CreateTextContentRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Text))
                return BadRequest("Text is required");
            
            var topic = await topicRepository.GetByIdAsync(request.TopicId);
            if (topic == null)
                return NotFound($"Topic with id {request.TopicId} not found");

            int order = await CalculateOrderAsync(request.TopicId);

            var content = new TextContent
            {
                TopicId = request.TopicId,
                Order = order,
                Text = request.Text
            };

            await contentItemRepository.AddAsync(content);
            return CreatedAtAction(nameof(GetContentById), new { id = content.Id }, content);
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

            var content = new ImageContent
            {
                TopicId = request.TopicId,
                Order = await CalculateOrderAsync(request.TopicId),
                ImageUrl = imageUrl
            };

            await contentItemRepository.AddAsync(content);

            return CreatedAtAction(nameof(GetContentById), new { id = content.Id }, content);
        }

        /// <summary>
        /// Создать ссылку как контент внутри топика.
        /// </summary>
        /// <param name="request">Объект запроса с URL и ID топика.</param>
        /// <returns>
        /// Возвращает созданный объект <see cref="LinkContent"/> с кодом 201,
        /// либо код 400/404 при ошибке валидации или отсутствии топика.
        /// </returns>
        [HttpPost("link")]
        public async Task<IActionResult> CreateLinkContent([FromBody] CreateLinkContentRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Url))
                return BadRequest("Link is required");
            
            var topic = await topicRepository.GetByIdAsync(request.TopicId);
            if (topic == null)
                return NotFound($"Topic with id {request.TopicId} not found");

            int order = await CalculateOrderAsync(request.TopicId);

            var content = new LinkContent()
            {
                TopicId = request.TopicId,
                Order = order,
                Url = request.Url
            };

            await contentItemRepository.AddAsync(content);
            return CreatedAtAction(nameof(GetContentById), new { id = content.Id }, content);
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

            return Ok(contentItem);
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
