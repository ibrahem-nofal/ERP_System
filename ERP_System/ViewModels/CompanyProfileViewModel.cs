using System.ComponentModel.DataAnnotations;

namespace ERP_System.ViewModels
{
    public class CompanyProfileViewModel
    {
        public int Id { get; set; }

        [Display(Name = "كود الشركة")]
        public string? Code { get; set; }

        [Required]
        [Display(Name = "اسم الشركة")]
        public string Name { get; set; }

        [Display(Name = "العنوان")]
        public string? Address { get; set; }

        [Display(Name = "الرقم الضريبي")]
        public string? TaxNumber { get; set; } // Stored in OtherDetails or separate field if added

        [Display(Name = "رقم التواصل")]
        public string? ContactNumber { get; set; }

        [Display(Name = "الشعار")]
        public IFormFile? LogoFile { get; set; } // For upload
        public string? LogoPath { get; set; }
    }
}
