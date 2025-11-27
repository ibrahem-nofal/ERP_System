using System.Collections.Generic;
using ERP_System.Models;
using System.ComponentModel.DataAnnotations;

namespace ERP_System.ViewModels
{
    public class InventorySettingsViewModel
    {
        public List<Unit>? Units { get; set; }
        public List<Category>? Categories { get; set; }

        [Display(Name = "طريقة تقييم المخزون")]
        public string? StockValuationMethod { get; set; } // FIFO, LIFO, Weighted Average
    }
}
