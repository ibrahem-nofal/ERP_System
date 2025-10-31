using System.ComponentModel.DataAnnotations;

namespace ERP_System.Models
{
    public class Supplier
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

        public ICollection<PurchaseInvoice> PurchaseInvoices { get; set; }
        public ICollection<Payment> Payments { get; set; }
    }
}
