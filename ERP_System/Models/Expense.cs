using System.ComponentModel.DataAnnotations;

namespace ERP_System.Models
{
    public class Expense
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int AccountId { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }

        public Account Account { get; set; }
    }
}
