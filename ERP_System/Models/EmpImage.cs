using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERP_System.Models
{
    public class EmpImage
    {
        [Key, ForeignKey("Employee")]
        public int EmpId { get; set; }

        public byte[] EmpImageData { get; set; }

        public Employee Employee { get; set; }
    }
}
