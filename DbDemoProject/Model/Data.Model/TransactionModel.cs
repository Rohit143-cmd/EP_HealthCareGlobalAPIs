using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DbDemoProject.Model.Data.Model
{
    [Table("Transactions")]
    public class TransactionModel
    {
        [Key]
        public Guid TransactionUUID { get; set; } = Guid.NewGuid();

        [Required]
        public Guid MemberUUID { get; set; }

        public Guid? AppointmentUUID { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }

        [Required]
        [StringLength(20)]
        public string TransactionType { get; set; } = "Payment";

        [StringLength(30)]
        public string Status { get; set; } = "Pending";

        [StringLength(100)]
        public string? PaymentMethod { get; set; }

        [StringLength(100)]
        public string? ReferenceNumber { get; set; }

        public DateTime TransactionDate { get; set; } = DateTime.UtcNow;

        public bool IsActive { get; set; } = true;

        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
    }
}
