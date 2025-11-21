using System;
using System.ComponentModel.DataAnnotations;
namespace ERP_System.Models
{
    public class Employee
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(150)]
        public string Name { get; set; }

        [Required]
        [MaxLength(20)]
        public string RoleType { get; set; }

        [Required]
        [MaxLength(10)]
        public string Gender { get; set; }

        [Required]
        [MaxLength(200)]
        public string Address { get; set; }

        [MaxLength(25)]
        public string IdNumber { get; set; }

        public DateTime? BirthDate { get; set; }

        [MaxLength(30)]
        public string Qualification { get; set; }

        [MaxLength(20)]
        public string State { get; set; }

        public ICollection<EmpPhone> Phones { get; set; }
        public EmpImage Image { get; set; }
        public ICollection<Login> Logins { get; set; }
    }
}
