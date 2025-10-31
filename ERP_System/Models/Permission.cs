using System.ComponentModel.DataAnnotations;

namespace ERP_System.Models
{
    public class Permission
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(100)]
        public string PermissionName { get; set; }

        public string Description { get; set; }

        // Navigation
        public ICollection<RolePermission> RolePermissions { get; set; }
    }
}
