namespace Backend.Entity;

  public class UserProgress
    {
        public int ProgressId { get; set; }
        public long UserId { get; set; }
        public int ContentId { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime? CompletionDate { get; set; }
    
        // Навигационные свойства
        public User User { get; set; }
        public ContentItem Content { get; set; }
    }
