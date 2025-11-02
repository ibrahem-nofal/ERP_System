using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ERP_System.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(150)]
        [Display(Name = "اسم المنتج")]
        public string Name { get; set; } = null!;

        [Required]
        [Display(Name = "الفئة")]
        public int CategoryId { get; set; }

        [ValidateNever]
        public Category? Category { get; set; }

        [Display(Name = "حد إعادة الطلب")]
        [Range(0, int.MaxValue, ErrorMessage = "يجب أن يكون رقمًا موجبًا")]
        public int? ReorderLevel { get; set; }

        [Display(Name = "سعر الوحدة")]
        [Range(0, double.MaxValue, ErrorMessage = "السعر يجب أن يكون رقمًا موجبًا")]
        public decimal UnitPrice { get; set; }

        [Display(Name = "الحالة")]
        public string Status { get; set; } = "غير متوفر";

        // ✅ الكود الداخلي (ITM-001)
        [Display(Name = "الكود")]
        [MaxLength(50)]
        public string Code { get; set; } = "";

        // ✅ الوصف التفصيلي للمنتج
        [Display(Name = "وصف المنتج")]
        [MaxLength(500, ErrorMessage = "الوصف لا يجب أن يتجاوز 500 حرف")]
        public string? Description { get; set; }

        [ValidateNever]
        public ICollection<StockLevel> StockLevels { get; set; } = new List<StockLevel>();

        [ValidateNever]
        public ICollection<StockTransferItem> StockTransferItems { get; set; } = new List<StockTransferItem>();
    }
}
