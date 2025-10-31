using System.ComponentModel.DataAnnotations;

namespace ERP_System.Models
{
    public class Warehouse
    {
        [Key]
        public int Id { get; set; }
        [Required, MaxLength(100)]
        public string Name { get; set; }
        [MaxLength(150)]
        public string Location { get; set; }

        public ICollection<StockLevel> StockLevels { get; set; }
        public ICollection<StockTransfer> FromTransfers { get; set; }
        public ICollection<StockTransfer> ToTransfers { get; set; }
    }
}

