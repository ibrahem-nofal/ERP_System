using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERP_System.Models
{
    public class CustomerPhone
    {
        public int CustomerId { get; set; }
        [MaxLength(50)]
        public string Phone { get; set; }

        [ForeignKey("CustomerId")]
        public Customer Customer { get; set; }
    }
}
