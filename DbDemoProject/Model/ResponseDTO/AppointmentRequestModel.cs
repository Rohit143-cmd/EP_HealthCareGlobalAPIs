using System.ComponentModel.DataAnnotations;

namespace DbDemoProject.Model.ResponseDTO
{
    public class AppointmentRequestModel
    {
        // Member Details
        public Guid? MemberUUID { get; set; }
        public Guid? AppointmentUUID { get; set; }

        [StringLength(100)]
        public string? MemberID { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string DoctorName { get; set; } = string.Empty;

        [StringLength(100)]
        public string? LastName { get; set; }

        [StringLength(255)]
        public string Email { get; set; } = string.Empty;

        [StringLength(20)]
        public string? PhoneNumber { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public string? Gender { get; set; }

        public string? Address { get; set; }

        // Appointment Details
        public DateTime? AppointmentDate { get; set; }
        public TimeOnly? AppointmentTime { get; set; }

        public Guid? AssignDoctorUUID { get; set; }

        public string PaymentMode { get; set; } = "Cash";

        public Guid? TransactionUUID { get; set; }

        public string? PaymentStatus { get; set; }

        // Audit Details
        public Guid? CreatedBy { get; set; }
    }
}
