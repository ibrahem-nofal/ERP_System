using System.ComponentModel.DataAnnotations;

namespace ERP_System.Models
{
    public class Setting
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Key { get; set; }

        public string Value { get; set; }

        [MaxLength(50)]
        public string Group { get; set; } // e.g., "Financial", "System", "Inventory"
    }
}
