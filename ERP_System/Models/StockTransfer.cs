using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERP_System.Models
{
    public class StockTransfer
    {
        public int Id { get; set; }

        [ForeignKey(nameof(FromWarehouse))]
        public int FromWarehouseId { get; set; }
        public Warehouse FromWarehouse { get; set; } = null!;

        [ForeignKey(nameof(ToWarehouse))]
        public int ToWarehouseId { get; set; }
        public Warehouse ToWarehouse { get; set; } = null!;

        public DateTime TransferDate { get; set; } = DateTime.Now;

        public string? Notes { get; set; }

        public string Status { get; set; } = "Pending";

        public ICollection<StockTransferItem> Items { get; set; } = new List<StockTransferItem>();
    }
}
