using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERP_System.Models
{
    public class Login
    {
        [Key, MaxLength(35)]
        public string Username { get; set; }

        [Required, MaxLength(35)]
        public string Password { get; set; }

        public bool? IsActive { get; set; }

        public int? EmpId { get; set; }
        [ForeignKey("EmpId")]
        public Employee Employee { get; set; }
    }
}
