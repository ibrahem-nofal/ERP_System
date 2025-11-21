using System.ComponentModel.DataAnnotations;

namespace ERP_System.ViewModels
{
    public class AddStoreVm
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string OtherDetails { get; set; }
    }
}
