
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DbDemoProject.Model.Data.Model
{
    [Table("Members")]
    public class MemberModel
    {
        [Key]
        public Guid MemberUUID { get; set; } = Guid.NewGuid();

        // SQL Server automatically generates 1, 2, 3, ...
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MemberNumber { get; private set; }

        // SQL Server generates MBR000001, MBR000002, ...
        [StringLength(23)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string MemberID { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string FirstName { get; set; } = string.Empty;

        [StringLength(100)]
        public string? LastName { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(150)]
        public string Email { get; set; } = string.Empty;

        [StringLength(20)]
        public string? PhoneNumber { get; set; }

        public DateTime? DateOfBirth { get; set; }
        //public DateTime? AppointmentDate { get; set; }

        [StringLength(20)]
        public string? Gender { get; set; }

        [StringLength(250)]
        public string? Address { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedDate { get; set; }
    }
}
