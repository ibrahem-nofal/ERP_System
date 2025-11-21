using System.ComponentModel.DataAnnotations;

namespace ERP_System.Models
{
    public class Supplier
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(200)]
        public string Name { get; set; }

        [MaxLength(200)]
        public string OwnerName { get; set; }

        [MaxLength(50)]
        public string OwnerPhone { get; set; }

        [MaxLength(200)]
        public string ManagerName { get; set; }

        [MaxLength(50)]
        public string ManagerPhone { get; set; }

        [MaxLength(250)]
        public string Address { get; set; }

        public ICollection<SupplierPhone> Phones { get; set; }
    }
}
