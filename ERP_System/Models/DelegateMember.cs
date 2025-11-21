using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERP_System.Models
{
    [Table("delegate")]
    public class DelegateMember
    {
        [Key]
        [ForeignKey("Employee")]
        public int EmpId { get; set; }

        public Employee Employee { get; set; }
    }
}
