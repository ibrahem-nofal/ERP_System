using System.ComponentModel.DataAnnotations;

namespace ERP_System.Models
{
    public class SalesInvoiceItem
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int SalesInvoiceId { get; set; }
        [Required]
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Total { get; set; }

        public SalesInvoice SalesInvoice { get; set; }
        public Product Product { get; set; }
    }
}
