using System.ComponentModel.DataAnnotations;

namespace ERP_System.Models
{
    public class PurchaseInvoice
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int SupplierId { get; set; }
        public DateTime InvoiceDate { get; set; }
        public decimal TotalAmount { get; set; }

        public Supplier Supplier { get; set; }
        public ICollection<PurchaseInvoiceItem> Items { get; set; }
        public ICollection<PurchaseReturn> Returns { get; set; }
    }
}
