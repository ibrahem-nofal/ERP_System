using System.ComponentModel.DataAnnotations;

namespace ERP_System.Models
{
    public class Receipt
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int AccountId { get; set; }
        public int? CustomerId { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public string Notes { get; set; }

        public Account Account { get; set; }
        public Customer Customer { get; set; }
    }
}
