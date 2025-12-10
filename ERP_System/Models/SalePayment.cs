using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERP_System.Models
{
    public class SalePayment
    {
        [Key]
        public int Id { get; set; }

        public int? InvoiceReturnId { get; set; }

        public int InvSaleId { get; set; }

        public decimal AmountPaid { get; set; }

        public DateTime PaymentDate { get; set; } = DateTime.Now;

        [MaxLength(50)]
        public string PaymentMethod { get; set; } // cash, visa, vCash, insta

        public string? Remarks { get; set; }

        public bool In { get; set; } = true;

        // Navigation properties
        [ForeignKey("InvSaleId")]
        public InvoiceSaleHeader? InvoiceSale { get; set; }
        [ForeignKey("InvoiceReturnId")]
        public InvoiceSaleHeader? InvoiceReturn { get; set; }
    }
}
