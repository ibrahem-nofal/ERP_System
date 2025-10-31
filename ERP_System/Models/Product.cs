using System.Collections.Generic;

namespace ERP_System.Models
{
    public class Product
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public string? SKU { get; set; }

        public int CategoryId { get; set; }
        public Category Category { get; set; } = null!;

        public int? ReorderLevel { get; set; }

        public decimal UnitPrice { get; set; }

        // ✅ العلاقات (Navigations)
        public ICollection<StockLevel> StockLevels { get; set; } = new List<StockLevel>();

        // ✅ دي اللي كانت ناقصة (العلاقة مع التحويلات)
        public ICollection<StockTransferItem> StockTransferItems { get; set; } = new List<StockTransferItem>();
    }
}
