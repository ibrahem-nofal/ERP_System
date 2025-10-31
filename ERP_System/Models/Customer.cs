using System.ComponentModel.DataAnnotations;

namespace ERP_System.Models
{
    public class Customer
    {
        [Key]
        public int Id { get; set; }
        [Required, MaxLength(150)]
        public string Name { get; set; }
        [MaxLength(50)]
        public string Phone { get; set; }
        [MaxLength(100)]
        public string Email { get; set; }
        public string Address { get; set; }

        public ICollection<SalesInvoice> SalesInvoices { get; set; }
        public ICollection<Receipt> Receipts { get; set; }
    }
}
