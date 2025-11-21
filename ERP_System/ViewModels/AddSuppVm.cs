using ERP_System.Models;

namespace ERP_System.ViewModels
{
    public class AddSuppVm
    {

        public int Id { get; set; }
        public string Name { get; set; }
        public string OwnerName { get; set; }
        public string OwnerPhone { get; set; }
        public string ManagerName { get; set; }
        public string ManagerPhone { get; set; }
        public string Address { get; set; }
        public List<string> Phones { get; set; }
    }
}
