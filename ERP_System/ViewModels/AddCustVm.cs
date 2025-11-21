using ERP_System.Models;

namespace ERP_System.ViewModels
{
    public class AddCustVm
    {

        public int Id { get; set; }
        public string Name { get; set; }
        public int Gender { get; set; }
        public string Address { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? BirthDate { get; set; }
        public string OtherDetails { get; set; }
        public List<string> Phones { get; set; }
    }
}
