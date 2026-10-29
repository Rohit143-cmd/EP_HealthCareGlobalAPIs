using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DbDemoProject.Model.Data.Model
{
    [Table("Appointments")]
    public class AppointmentModel
    {
        [Key]
        public Guid AppointmentUUID { get; set; }

        [Required]
        public Guid MemberUUID { get; set; }
        public Guid? DoctorUUID { get; set; }

        [Required]
        public DateTime AppointmentDate { get; set; }
        public TimeOnly? AppointmentTime { get; set; }

        [StringLength(100)]
        public string? AppointmentType { get; set; }

        [StringLength(500)]
        public string? Description { get; set; }

        [StringLength(30)]
        public string Status { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime? UpdatedOn { get; set; }
    }
}
