using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERP_System.Models
{
    public class ActivityLog
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(35)]
        public string Username { get; set; }

        public DateTime? ActDate { get; set; }
        public TimeSpan? ActTime { get; set; }

        [MaxLength(255)]
        public string FormName { get; set; }

        [MaxLength(255)]
        public string OpName { get; set; }

        [MaxLength(255)]
        public string CmpName { get; set; }

        public string ActivityData { get; set; }

        [ForeignKey("Username")]
        public Login Login { get; set; }
    }
}
