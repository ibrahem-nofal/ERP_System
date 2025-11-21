using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERP_System.Models
{
    public class Store
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(150)]
        public string Name { get; set; }

        [MaxLength(200)]
        public string Address { get; set; }

        [MaxLength(50)]
        public string Phone { get; set; }

        public string OtherDetails { get; set; }

        public int? StoreManager { get; set; }

        [ForeignKey("StoreManager")]
        public Employee Manager { get; set; }
    }
}
