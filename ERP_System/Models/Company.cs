using System.ComponentModel.DataAnnotations;

namespace ERP_System.Models
{
    public class Company
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(50)]
        public string Code { get; set; }

        [MaxLength(150)]
        public string Name { get; set; }

        [MaxLength(200)]
        public string Address { get; set; }

        public DateTime? DateCreated { get; set; }

        public string OtherDetails { get; set; }

        public ICollection<CompanyPhone> Phones { get; set; }
    }
}
