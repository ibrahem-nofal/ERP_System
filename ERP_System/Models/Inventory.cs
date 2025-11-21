namespace ERP_System.Models
{
    public class Inventory
    {
        public int StoreId { get; set; }

        public int ItemId { get; set; }

        public int CurrentQuantity { get; set; } = 0;

        public int OrderedQuantity { get; set; } = 0;

        public DateTime LastUpdated { get; set; } = DateTime.Now;

        // Navigation properties
        public Store Store { get; set; }
        public Item Item { get; set; }
    }

}
