using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERP_System.Models
{
    public class Item
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(250)]
        public string Name { get; set; }

        public bool? IsActiveBuy { get; set; }
        public bool? IsActiveSale { get; set; }

        public int? CompanyMade { get; set; }
        [ForeignKey("CompanyMade")]
        public Company Company { get; set; }

        public int? DefaultStore { get; set; }
        [ForeignKey("DefaultStore")]
        public Store Store { get; set; }

        public int? UnitNumber { get; set; }
        [ForeignKey("UnitNumber")]
        public Unit Unit { get; set; }

        public int? MinimumQuantity { get; set; }
        public int? MinQuantitySale { get; set; }

        public bool PreventFraction { get; set; } = true;
        public bool PreventDiscount { get; set; } = true;

        public decimal? BuyPrice { get; set; }
        public decimal? SalePrice { get; set; }

        public ICollection<ItemCode> Codes { get; set; }
        public ICollection<ItemImage> Images { get; set; }
    }
}
