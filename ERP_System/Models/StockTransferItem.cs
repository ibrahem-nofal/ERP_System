using System.ComponentModel.DataAnnotations;

namespace ERP_System.Models
{
    public class StockTransferItem
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int TransferId { get; set; }
        [Required]
        public int ProductId { get; set; }
        [Required]
        public int Quantity { get; set; }

        public StockTransfer Transfer { get; set; }
        public Product Product { get; set; }
    }
}
