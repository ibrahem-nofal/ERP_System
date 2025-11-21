using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERP_System.Models
{
    public class ItemCode
    {
        public int ItemId { get; set; }

        [MaxLength(200)]
        public string ItemCodeValue { get; set; }

        [ForeignKey("ItemId")]
        public Item Item { get; set; }
    }
}
