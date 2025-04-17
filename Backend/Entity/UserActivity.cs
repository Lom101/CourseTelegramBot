namespace Backend.Entity;
 public class UserActivity
    {
        public int ActivityId { get; set; }
        public long UserId { get; set; }
        public string ActionType { get; set; } // "button_click", "link_click", "test_attempt"
        public string Details { get; set; }
        public DateTime ActivityDate { get; set; }
    
        // Навигационное свойство
        public User User { get; set; }
    }
