using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERP_System.Models
{
    public class InvoiceSaleDetail
    {
        [Key]
        public int Id { get; set; }

        public int InvoiceId { get; set; }

        public int? InvReturnId { get; set; }

        public int ItemId { get; set; }

        public int Quantity { get; set; }

        public decimal UnitPrice { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public decimal TotalPrice { get; private set; }

        [MaxLength(10)]
        public string Status { get; set; } // Purchased, Returned

        // Navigation properties
        public InvoiceSaleHeader Invoice { get; set; }
        public Item Item { get; set; }
        [ForeignKey("InvReturnId")]
        public InvoiceSaleHeader InvReturn { get; set; }
    }
}
