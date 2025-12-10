using System.ComponentModel.DataAnnotations;

namespace ERP_System.Models
{
    public class InvoicePurchaseHeader
    {
        public int Id { get; set; }

        public int? RefInvId { get; set; }

        public string? InvType { get; set; } 

        public DateTime DateCreated { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "المورد مطلوب")]
        public int? SupplierId { get; set; }

        [Required(ErrorMessage = "المخزن مطلوب")]
        public int? StoreId { get; set; }

        [Required(ErrorMessage = "حالة الطلب مطلوبة")]
        public string OrderStatus { get; set; } 

        public DateTime? DeliveryDate { get; set; }

        public int? AssignedBy { get; set; }

        public decimal TotalAmount { get; set; }

        public decimal Discount { get; set; } = 0;

        
        public decimal NetAmount { get; private set; }

        public decimal Paid { get; set; } = 0;

        public decimal Remain { get; set; } = 0;

        public string? Remarks { get; set; }

        [Required(ErrorMessage = "حالة الدفع مطلوبة")]
        public string PayStatus { get; set; } // open, closed

        public bool IsPostpaid { get; set; } = false;

        public DateTime? PaymentDueDate { get; set; }

        // Navigation properties (optional)
        public Supplier? Supplier { get; set; }
        public Store? Store { get; set; }
        public Employee? AssignedByEmployee { get; set; }
        public InvoicePurchaseHeader? RefInvoice { get; set; }
    }
}
