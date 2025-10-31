using System.ComponentModel.DataAnnotations;

namespace ERP_System.Models
{
    public class SalesReturn
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int SalesInvoiceId { get; set; }
        public DateTime ReturnDate { get; set; }
        public decimal TotalAmount { get; set; }

        public SalesInvoice SalesInvoice { get; set; }
        public ICollection<SalesReturnItem> Items { get; set; }
    }
}
