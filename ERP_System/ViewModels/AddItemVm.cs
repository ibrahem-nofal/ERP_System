using ERP_System.Models;

namespace ERP_System.ViewModels
{
    public class AddItemVm
    {
        public string Name { get; set; }
        public bool IsActiveBuy { get; set; }
        public bool IsActiveSale { get; set; }
        public int? CompanyMade { get; set; }
        public int? DefaultStore { get; set; }
        public int? UnitNumber { get; set; }
        public int? MinimumQuantity { get; set; }
        public int? MinQuantitySale { get; set; }
        public bool PreventFraction { get; set; } = true;
        public bool PreventDiscount { get; set; } = true;
        public decimal? BuyPrice { get; set; }
        public decimal? SalePrice { get; set; }
        public List<string> Codes { get; set; }

        public List<int> CategoryIds { get; set; }
        public IFormFile Image { get; set; }
    }
}
