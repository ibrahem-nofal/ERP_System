using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERP_System.Models
{
    public class ItemImage
    {
        [Key, ForeignKey("Item")]
        public int ItemId { get; set; }

        public byte[] ItemImageData { get; set; }

        public Item Item { get; set; }
    }
}
