using ERP_System.Models;

namespace ERP_System.ViewModels
{
    public class AddCustVm
    {

        public int Id { get; set; }
        [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "يرجى إدخال اسم العميل")]
        public string Name { get; set; }
        public int Gender { get; set; }
        [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "يرجى إدخال العنوان")]
        public string Address { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? BirthDate { get; set; }
        public string? OtherDetails { get; set; }
        public List<string> Phones { get; set; }
    }
}
