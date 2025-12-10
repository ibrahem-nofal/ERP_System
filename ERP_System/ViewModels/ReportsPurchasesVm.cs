namespace ERP_System.ViewModels
{
    public class ReportsPurchasesVm
    {
        public decimal TotalPurchases { get; set; }
        public int InvoiceCount { get; set; }
        public int NewSuppliersCount { get; set; }
        public decimal TotalPayments { get; set; }
        public decimal TotalPurchasesComparisonPercentage { get; set; }
        public Dictionary<string, decimal> MonthlyPurchases { get; set; } = new Dictionary<string, decimal>();
        public Dictionary<string, decimal> TopSuppliers { get; set; } = new Dictionary<string, decimal>(); // SupplierName -> TotalAmount
    }
}
