using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ERP_System.Models
{
    public class Warehouse
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(100)]
        [Display(Name = "اسم المخزن")]
        public string Name { get; set; }

        [MaxLength(150)]
        [Display(Name = "الموقع")]
        public string Location { get; set; }

        // 🟢 الحالة (نشط / غير نشط)
        [Display(Name = "الحالة")]
        public bool IsActive { get; set; } = true;

        // 🟢 السعة التخزينية الكلية للمخزن
        [Display(Name = "السعة التخزينية")]
        [Range(0, int.MaxValue, ErrorMessage = "السعة يجب أن تكون رقمًا موجبًا")]
        public int Capacity { get; set; } = 0;

        // 🟢 العلاقات
        public ICollection<StockLevel>? StockLevels { get; set; }
        public ICollection<StockTransfer>? FromTransfers { get; set; }
        public ICollection<StockTransfer>? ToTransfers { get; set; }
    }
}
