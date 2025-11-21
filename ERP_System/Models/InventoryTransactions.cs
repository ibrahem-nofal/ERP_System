using System.ComponentModel.DataAnnotations;

namespace ERP_System.Models
{
    public class InventoryTransaction
    {
        [Key]
        public int TransactionId { get; set; }

        public int ItemId { get; set; }

        public string TransactionType { get; set; } // Purchase, PurchaseReturn, Sales, SalesReturn, Adjustment

        public int Quantity { get; set; }

        public int StoreId { get; set; }

        public DateTime TransactionDate { get; set; } = DateTime.Now;

        public int? ReferenceId { get; set; }

        public int? AssignedBy { get; set; }

        public string Remarks { get; set; }

        // Navigation properties
        public Item Item { get; set; }
        public Store Store { get; set; }
        public Employee AssignedByEmployee { get; set; }
    }

}
