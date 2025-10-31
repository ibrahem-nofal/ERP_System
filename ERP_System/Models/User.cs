using System.ComponentModel.DataAnnotations;

namespace ERP_System.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(50)]
        public string Username { get; set; }

        [Required, MaxLength(255)]
        public string PasswordHash { get; set; }

        [MaxLength(100)]
        public string Email { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Navigation Properties
        public ICollection<UserRole> UserRoles { get; set; }
        public ICollection<ActivityLog> ActivityLogs { get; set; }
    }
}
