using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERP_System.Models
{
    public class RolePermission
    {
        [Key, Column(Order = 0)]
        public int RoleId { get; set; }

        [Key, Column(Order = 1)]
        public int PermissionId { get; set; }

        // Navigation
        public Role Role { get; set; }
        public Permission Permission { get; set; }
    }
}
