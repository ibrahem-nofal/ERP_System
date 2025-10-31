using System.ComponentModel.DataAnnotations;

namespace ERP_System.Models
{
    public class ActivityLog
    {
        [Key]
        public int Id { get; set; }

        public int? UserId { get; set; }
        public string Action { get; set; }
        public string EntityName { get; set; }
        public int EntityId { get; set; }

        [Required]
        public DateTime Timestamp { get; set; } = DateTime.Now;

        // Navigation
        public User User { get; set; }
    }
}
