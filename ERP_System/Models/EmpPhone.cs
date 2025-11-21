using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERP_System.Models
{
    public class EmpPhone
    {
        public int EmpId { get; set; }

        [MaxLength(20)]
        public string Phone { get; set; }

        [ForeignKey("EmpId")]
        public Employee Employee { get; set; }
    }

}
