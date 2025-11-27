namespace ERP_System.ViewModels
{
    public class AddCategoryVm
    {
        [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "يرجى إدخال اسم البند")]
        public string Name { get; set; }

        public string Detail { get; set; }
    }
}
