using ERP_System.Models;

namespace ERP_System.ViewModels
{
    public class AddEmpVm
    {
        public string Name { get; set; }
        public int RoleType { get; set; }
        public int Gender { get; set; }
        public string Address { get; set; }
        public string IdNumber { get; set; }
        public DateTime? BirthDate { get; set; }
        public int Qualification { get; set; }
        public int State { get; set; }
        public List<string> Phones { get; set; }
        public IFormFile EmpImage { get; set; }
    }
}
