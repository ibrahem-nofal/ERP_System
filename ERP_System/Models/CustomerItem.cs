using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERP_System.Models
{
    public class CustomerItem
    {
        [Key, Column(Order = 0)]
        public int CustomerId { get; set; }

        [Key, Column(Order = 1)]
        public int ItemId { get; set; }

        public Customer Customer { get; set; }
        public Item Item { get; set; }
    }
}
