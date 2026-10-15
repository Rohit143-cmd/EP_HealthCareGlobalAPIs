using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DbDemoProject.Model
{
    public class NavigationMenu
    {
        [Key]
        public Guid? NavigationMenuUUID { get; set; }
        public Guid? RoleUUID { get; set; }
        [ForeignKey(nameof(RoleUUID))]
        public UserRoles? Role { get; set; }
        [StringLength(100)]
        public string DisplayName { get; set; }
        [StringLength(500)]
        public string URL { get; set; }
        public bool IsActive { get; set; }
        public bool IsParent { get; set; }
        public int Order {  get; set; }
        public Guid? ParentUUID { get; set; }
    }
}