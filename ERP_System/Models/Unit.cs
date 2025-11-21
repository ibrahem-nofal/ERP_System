using System.ComponentModel.DataAnnotations;

namespace ERP_System.Models
{
    public class Unit
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(150)]
        public string Name { get; set; }

        public string Details { get; set; }
    }
}
