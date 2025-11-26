using System.ComponentModel.DataAnnotations;

namespace ERP_System.ViewModels
{
    public class AddItemVm
    {
        [Required(ErrorMessage = "الرجاء إدخال اسم الصنف.")]
        [StringLength(250, ErrorMessage = "اسم الصنف لا يجب أن يزيد عن 250 حرفاً.")]
        public string Name { get; set; }

        public string? Description { get; set; }

        public bool IsActiveBuy { get; set; } = true;
        public bool IsActiveSale { get; set; } = true;

        public int? CompanyMade { get; set; }
        public int? DefaultStore { get; set; }
        public int? UnitNumber { get; set; }

        public int? MinimumQuantity { get; set; }
        public int? MinQuantitySale { get; set; }

        public bool PreventFraction { get; set; } = true;
        public bool PreventDiscount { get; set; } = true;

        [Range(0, double.MaxValue, ErrorMessage = "سعر الشراء يجب أن يكون رقماً غير سالب.")]
        public decimal? BuyPrice { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "سعر البيع يجب أن يكون رقماً غير سالب.")]
        public decimal? SalePrice { get; set; }

        // تهيئة القوائم لتفادي null
        public List<string> Codes { get; set; } = new List<string>();

        public List<int> CategoryIds { get; set; } = new List<int>();

        // Image قد تكون null
        public IFormFile? Image { get; set; }
    }
}
