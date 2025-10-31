using System.ComponentModel.DataAnnotations;

namespace ERP_System.Models
{
    public class PurchaseReturn
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int PurchaseInvoiceId { get; set; }
        public DateTime ReturnDate { get; set; }
        public decimal TotalAmount { get; set; }

        public PurchaseInvoice PurchaseInvoice { get; set; }
        public ICollection<PurchaseReturnItem> Items { get; set; }
    }
}
