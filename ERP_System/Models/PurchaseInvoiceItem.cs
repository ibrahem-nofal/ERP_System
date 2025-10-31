using System.ComponentModel.DataAnnotations;

namespace ERP_System.Models
{
    public class PurchaseInvoiceItem
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int PurchaseInvoiceId { get; set; }
        [Required]
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Total { get; set; }

        public PurchaseInvoice PurchaseInvoice { get; set; }
        public Product Product { get; set; }
    }
}
