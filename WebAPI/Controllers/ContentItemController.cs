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
        /// Получить контент по ID.
        /// </summary>
        /// <param name="id">ID контента.</param>
        /// <returns>Контент с заданным ID.</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetContentById(int id)
        {
            var contentItem = await contentItemRepository.GetByIdAsync(id);
            return contentItem != null ? Ok(contentItem) : NotFound($"Content item with id {id} not found");
        }

        /// <summary>
        /// Получить все контент-элементы по ID топика.
        /// </summary>
        /// <param name="topicId">ID топика.</param>
        /// <returns>Список контента для указанного топика.</returns>
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
        /// Обновить порядок контента в рамках топика.
        /// </summary>
        /// <param name="id">ID контента для обновления.</param>
        /// <param name="newOrder">Новый порядок.</param>
        /// <returns>Обновленный контент.</returns>
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
        /// Удалить контент.
        /// </summary>
        /// <param name="id">ID контента для удаления.</param>
        /// <returns>Результат операции удаления.</returns>
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
