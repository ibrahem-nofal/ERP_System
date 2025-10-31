using System.ComponentModel.DataAnnotations;

namespace ERP_System.Models
{
    public class PurchaseReturnItem
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int PurchaseReturnId { get; set; }
        [Required]
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }

        public PurchaseReturn PurchaseReturn { get; set; }
        public Product Product { get; set; }
    }
}
