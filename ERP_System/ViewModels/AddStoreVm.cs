using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace ERP_System.ViewModels
{
    public class AddStoreVm
    {
        [Required(ErrorMessage = "الرجاء إدخال اسم المخزن.")]
        [StringLength(150, ErrorMessage = "الاسم يجب ألا يتجاوز 150 حرف.")]
        public string Name { get; set; }


        [StringLength(250, ErrorMessage = "العنوان يجب ألا يتجاوز 250 حرف.")]
        public string? Address { get; set; }

        [StringLength(120, ErrorMessage = "اسم المدير يجب ألا يتجاوز 120 حرف.")]
        public string? ManagerName { get; set; }

        public List<string> Phones { get; set; } = new List<string>();

        [EmailAddress(ErrorMessage = "صيغة البريد الإلكتروني غير صحيحة.")]
        public string? Email { get; set; }

        public bool IsActive { get; set; } = true;

        [StringLength(200, ErrorMessage = "أوقات العمل طويلة جداً.")]
        public string? OpeningHours { get; set; }

        [StringLength(2000, ErrorMessage = "الملاحظات طويلة جداً.")]
        public string? Notes { get; set; }

        // صورة المخزن (اختياري)
        public IFormFile? Image { get; set; }
    }
}
