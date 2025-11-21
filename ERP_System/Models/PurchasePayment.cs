
using System.ComponentModel.DataAnnotations;

namespace ERP_System.Models
{
    public class PurchasePayment
    {
        public int Id { get; set; }

        public int? InvoiceReturnId { get; set; }

        public int InvPurchaseId { get; set; }

        public decimal AmountPaid { get; set; }

        public DateTime PaymentDate { get; set; } = DateTime.Now;

        public string PaymentMethod { get; set; }  // cash, visa, vCash, insta

        public string Remarks { get; set; }

        public bool Out { get; set; } = true;

        // Navigation properties
        public InvoicePurchaseHeader InvoiceReturn { get; set; }
        public InvoicePurchaseHeader InvoicePurchase { get; set; }
    }
}