namespace ERP_System.ViewModels
{
    public class AddRevenueNameVm
    {
        [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "يرجى إدخال اسم البند")]
        public string Name { get; set; }

        [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "يرجى إدخال التفاصيل")]
        public string Detail { get; set; }
    }
}
