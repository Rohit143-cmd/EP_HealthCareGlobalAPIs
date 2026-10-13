using System.ComponentModel.DataAnnotations;

namespace DbDemoProject.Model
{
    public class UserRoles
    {
        [Key]
        public Guid RoleUUID { get; set; }
        public string RoleName { get; set; }
        public DateTime CreatedOn { get; set; }
        public bool IsActive { get; set; }
    }
}
