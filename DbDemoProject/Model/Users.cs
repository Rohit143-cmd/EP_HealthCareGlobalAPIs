using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DbDemoProject.Model
{
    public class Users
    {
        [Key]
        public Guid? UserUUID { get; set; }
        public Guid? RoleUUID { get; set; }
        [ForeignKey(nameof(RoleUUID))]
        public UserRoles? Role { get; set; }
        public Guid? ProductUUID { get; set; }
        [ForeignKey(nameof(ProductUUID))]
        public Product? Product { get; set; }
        [StringLength(100)]
        [Required]
        [EmailAddress]
        public string UserName { get; set; }
        [StringLength(100)]
        [EmailAddress]
        public string EmailID { get; set; }
        [StringLength(10)]
        [Required]
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string PasswordHash { get; set; }
        public bool IsActive { get; set; }
    }
}
