using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERP_System.Models
{
    public class InvoiceSaleHeader
    {
        [Key]
        public int Id { get; set; }

        public int? RefInvId { get; set; }

        [MaxLength(20)]
        public string InvType { get; set; } // SalesCash, SalesReturnCash, SalesCredit, SalesReturnCredit

        public DateTime DateCreated { get; set; } = DateTime.Now;

        public int? CustomerId { get; set; }

        public int? DelegateId { get; set; }

        public int? StoreId { get; set; }

        [MaxLength(20)]
        public string OrderStatus { get; set; } // wait, sent, returned, returning

        public DateTime? DeliveryDate { get; set; }

        public int? AssignedBy { get; set; }

        public decimal TotalAmount { get; set; }

        public decimal Discount { get; set; } = 0;

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public decimal NetAmount { get; private set; }

        public decimal Paid { get; set; } = 0;

        public decimal Remain { get; set; } = 0;

        public string Remarks { get; set; }

        [MaxLength(20)]
        public string PayStatus { get; set; } // open, closed

        public bool IsPostpaid { get; set; } = false;

        public DateTime? PaymentDueDate { get; set; }

        // Navigation properties
        public Customer Customer { get; set; }
        public DelegateMember Delegate { get; set; }
        public Store Store { get; set; }
        public Employee AssignedByEmployee { get; set; }
        [ForeignKey("RefInvId")]
        public InvoiceSaleHeader RefInvoice { get; set; }
    }
}
