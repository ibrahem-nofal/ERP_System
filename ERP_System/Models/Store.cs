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

        [MaxLength(250)]
        public string? Address { get; set; }

        [MaxLength(120)]
        public string? ManagerName { get; set; }

        public ICollection<StorePhone> Phones { get; set; }

        [MaxLength(120)]
        public string? Email { get; set; }

        public bool IsActive { get; set; } = true;

        [MaxLength(200)]
        public string? OpeningHours { get; set; }

        public string? Notes { get; set; }



        // صور (1..n)
        public ICollection<StoreImage> Images { get; set; }

        public ICollection<Inventory> Inventories { get; set; } = new List<Inventory>();
    }

    public class StorePhone
    {
        [Key]
        public int Id { get; set; }
        public int StoreId { get; set; }
        [ForeignKey("StoreId")]
        public Store Store { get; set; }
        [MaxLength(50)]
        public string Phone { get; set; }
    }

    public class StoreImage
    {
        [Key]
        public int Id { get; set; }
        public int StoreId { get; set; }
        [ForeignKey("StoreId")]
        public Store Store { get; set; }
        public byte[] ImageData { get; set; }
    }
}
