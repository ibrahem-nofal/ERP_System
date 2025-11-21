using ERP_System.Models;

namespace ERP_System.ViewModels
{
    public class AddCompVm
    {

        public string Code { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public DateTime? DateCreated { get; set; }
        public string OtherDetails { get; set; }
        public List<string> Phones { get; set; }

    }
}
