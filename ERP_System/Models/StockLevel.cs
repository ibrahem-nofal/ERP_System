using System.ComponentModel.DataAnnotations;

namespace ERP_System.Models
{
    public class StockLevel
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int ProductId { get; set; }
        [Required]
        public int WarehouseId { get; set; }
        public int Quantity { get; set; } = 0;

        public Product Product { get; set; }
        public Warehouse Warehouse { get; set; }
    }
}
