using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERP_System.Models
{
    public class ItemCategory
    {
        public int ItemId { get; set; }
        public int CategoryId { get; set; }

        [ForeignKey(nameof(ItemId))]
        public Item Item { get; set; }

        [ForeignKey(nameof(CategoryId))]
        public Category Category { get; set; }
    }
}
