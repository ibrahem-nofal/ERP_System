using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERP_System.Models
{
    public class StockTransfer
    {
        public int Id { get; set; }

        [Display(Name = "من مخزن")]
        public int FromWarehouseId { get; set; }

        [ForeignKey(nameof(FromWarehouseId))]
        public Warehouse FromWarehouse { get; set; } = null!;

        [Display(Name = "إلى مخزن")]
        public int ToWarehouseId { get; set; }

        [ForeignKey(nameof(ToWarehouseId))]
        public Warehouse ToWarehouse { get; set; } = null!;

        [Display(Name = "تاريخ التحويل")]
        public DateTime TransferDate { get; set; } = DateTime.Now;

        [Display(Name = "الملاحظات")]
        public string? Notes { get; set; }

        [Display(Name = "الحالة")]
        public string Status { get; set; } = "قيد التنفيذ";

        [Display(Name = "تفاصيل التحويل")]
        public ICollection<StockTransferItem> Items { get; set; } = new List<StockTransferItem>();
    }
}
