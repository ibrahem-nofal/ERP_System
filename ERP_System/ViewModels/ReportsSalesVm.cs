namespace ERP_System.ViewModels
{
    public class ReportsSalesVm
    {
        public decimal TotalSales { get; set; }
        public int InvoiceCount { get; set; }
        public int NewCustomersCount { get; set; }
        public decimal AverageInvoiceValue { get; set; }
        public decimal TotalSalesComparisonPercentage { get; set; } // vs Last Month
        public Dictionary<string, decimal> MonthlySales { get; set; } = new Dictionary<string, decimal>();
        public Dictionary<string, int> TopSellingItems { get; set; } = new Dictionary<string, int>(); // ItemName -> Quantity
    }
}
