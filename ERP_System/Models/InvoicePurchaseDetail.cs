using System.ComponentModel.DataAnnotations;
namespace ERP_System.Models
{
    public class InvoicePurchaseDetail
    {
        public int Id { get; set; }

        public int InvoiceId { get; set; }

        public int? InvReturnId { get; set; }

        public int ItemId { get; set; }

        public int Quantity { get; set; }

        public decimal UnitPrice { get; set; }

        // Computed column → EF should treat it as read-only
        public decimal TotalPrice { get; private set; }

        public string Status { get; set; } // Purchased, Returned

        // Navigation properties
        public InvoicePurchaseHeader Invoice { get; set; }
        public InvoicePurchaseHeader ReturnedInvoice { get; set; }
        public Item Item { get; set; }
    }
}