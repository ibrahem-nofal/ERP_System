using System.ComponentModel.DataAnnotations;

namespace ERP_System.Models
{
    public class Customer
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(150)]
        public string Name { get; set; }

        [MaxLength(10)]
        public string Gender { get; set; }

        [MaxLength(250)]
        public string Address { get; set; }

        public DateTime? StartDate { get; set; }
        public DateTime? BirthDate { get; set; }

        public string OtherDetails { get; set; }

        public ICollection<CustomerPhone> Phones { get; set; }
    }
}
