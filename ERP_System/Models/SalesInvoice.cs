using System.ComponentModel.DataAnnotations;

namespace ERP_System.Models
{
    public class SalesInvoice
    {

        [Key]
        public int Id { get; set; }
        [Required]
        public int CustomerId { get; set; }
        public DateTime InvoiceDate { get; set; }
        public decimal TotalAmount { get; set; }
        [MaxLength(50)]
        public string Status { get; set; }

        public Customer Customer { get; set; }
        public ICollection<SalesInvoiceItem> Items { get; set; }
        public ICollection<SalesReturn> Returns { get; set; }
    }
}
