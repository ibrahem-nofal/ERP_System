namespace ERP_System.ViewModels
{
    public class ReportsInventoryVm
    {
        public decimal TotalInventoryValue { get; set; }
        public int TotalItemsCount { get; set; }
        public int LowStockItemsCount { get; set; }
        public int InventoryMovementsCount { get; set; } // Today's movements
        public Dictionary<string, int> InventoryByCategory { get; set; } = new Dictionary<string, int>();
    }
}
