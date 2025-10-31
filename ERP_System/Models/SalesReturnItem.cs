using System.ComponentModel.DataAnnotations;

namespace ERP_System.Models
{
    public class SalesReturnItem
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int SalesReturnId { get; set; }
        [Required]
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }

        public SalesReturn SalesReturn { get; set; }
        public Product Product { get; set; }
    }
}
