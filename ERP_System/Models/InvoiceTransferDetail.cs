using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERP_System.Models
{
    public class InvoiceTransferDetail
    {
        [Key]
        public int Id { get; set; }

        public int HeaderId { get; set; }

        public int ItemId { get; set; }

        public int Quantity { get; set; }

        [ForeignKey("HeaderId")]
        public virtual InvoiceTransferHeader? Header { get; set; }

        [ForeignKey("ItemId")]
        public virtual Item? Item { get; set; }
    }
}
