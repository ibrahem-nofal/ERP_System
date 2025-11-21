using System.ComponentModel.DataAnnotations;

namespace ERP_System.Models
{
    public class ExpenseName
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(200)]
        public string Name { get; set; }

        public string Description { get; set; }
    }
}
