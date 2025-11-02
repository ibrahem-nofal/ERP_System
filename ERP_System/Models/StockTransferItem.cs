using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERP_System.Models
{
    public class StockTransferItem
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "رقم التحويل")]
        [ForeignKey(nameof(Transfer))]
        public int TransferId { get; set; }

        [Display(Name = "المنتج")]
        [ForeignKey(nameof(Product))]
        public int ProductId { get; set; }

        [Display(Name = "الكمية")]
        [Range(1, int.MaxValue, ErrorMessage = "يجب أن تكون الكمية أكبر من صفر")]
        public int Quantity { get; set; }

        public StockTransfer Transfer { get; set; } = null!;
        public Product Product { get; set; } = null!;
    }
}
