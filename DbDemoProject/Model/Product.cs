using System.ComponentModel.DataAnnotations;

namespace DbDemoProject.Model
{
    public class Product
    {
        [Key]
        public Guid? ProductUUID { get; set; }
        [StringLength(100)]
        [Required]
        public string ProductName { get; set; }
        public DateTime CreatedOn { get; set; }
        public bool IsActive { get; set; }
    }
}
