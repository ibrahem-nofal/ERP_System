namespace ERP_System.Models
{
    public class JournalEntry
    {
        public int Id { get; set; }

        public string Description { get; set; }

        public string SourceType { get; set; }   // PurchaseCash, SalesCash, Revenue, Expense ...

        public int? InvPurId { get; set; }

        public int? InvSaleId { get; set; }

        public int AssignedBy { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Navigation properties
        public InvoicePurchaseHeader InvoicePurchase { get; set; }
         public InvoiceSaleHeader InvoiceSale { get; set; }
        public Employee AssignedByEmployee { get; set; }
        public ICollection<JournalDetail> Details { get; set; }
    }

}
