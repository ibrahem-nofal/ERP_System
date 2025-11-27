using System.ComponentModel.DataAnnotations;

namespace ERP_System.ViewModels
{
    public class FinancialSettingsViewModel
    {
        [Display(Name = "العملة")]
        public string? Currency { get; set; }

        [Display(Name = "نسبة الضريبة الافتراضية")]
        public decimal TaxPercentage { get; set; }

        [Display(Name = "بداية السنة المالية")]
        public DateTime FiscalYearStart { get; set; }

        [Display(Name = "تفعيل الترقيم التلقائي")]
        public bool AutoNumbering { get; set; }
    }
}
