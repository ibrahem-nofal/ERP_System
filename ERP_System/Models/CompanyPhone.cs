using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERP_System.Models
{
    public class CompanyPhone
    {
        public int CompId { get; set; }

        [MaxLength(60)]
        public string Phone { get; set; }

        [ForeignKey("CompId")]
        public Company Company { get; set; }
    }
}
