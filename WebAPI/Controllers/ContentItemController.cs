using Core.Entity;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Controllers
{
    /// <summary>
    /// Контроллер для управления контентом внутри топиков.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class ContentItemController(IContentItemRepository contentItemRepository, ITopicRepository topicRepository)
        : ControllerBase
    {
        
        /// <summary>
        /// Получить контент по ID.
        /// </summary>
        /// <param name="id">ID контента.</param>
        /// <returns>Контент с заданным ID.</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<ContentItem>> GetContentById(long id)
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
        public async Task<ActionResult<IEnumerable<ContentItem>>> GetContentByTopicId(long topicId)
        {
            var contentItems = await contentItemRepository.GetByTopicIdAsync(topicId);
            var sortedContentItems = contentItems.OrderBy(c => c.Order).ToList(); // Сортируем по порядку
            return sortedContentItems.Any() ? Ok(sortedContentItems) : NotFound($"No content found for topic with id {topicId}");
        }

        /// <summary>
        /// Создать новый контент.
        /// </summary>
        /// <param name="contentItem">Данные контент-элемента.</param>
        /// <returns>Созданный контент.</returns>
        [HttpPost]
        public async Task<IActionResult> CreateContent([FromBody] ContentItem contentItem)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid content data");

            // Получаем все контент-элементы для текущего топика и находим максимальный порядок
            var contentItems = await contentItemRepository.GetByTopicIdAsync(contentItem.TopicId);
            contentItem.Order = contentItems.Any() ? contentItems.Max(c => c.Order) + 1 : 1;

            await contentItemRepository.AddAsync(contentItem);
            return CreatedAtAction(nameof(GetContentById), new { id = contentItem.Id }, contentItem);
        }

        /// <summary>
        /// Обновить порядок контента в рамках топика.
        /// </summary>
        /// <param name="id">ID контента для обновления.</param>
        /// <param name="newOrder">Новый порядок.</param>
        /// <returns>Обновленный контент.</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateContentOrder(long id, [FromBody] int newOrder)
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
        public async Task<IActionResult> DeleteContent(long id)
        {
            var contentItem = await contentItemRepository.GetByIdAsync(id);
            if (contentItem == null)
                return NotFound($"Content item with id {id} not found");

            await contentItemRepository.DeleteAsync(contentItem);
            return NoContent();
        }
    }
}
