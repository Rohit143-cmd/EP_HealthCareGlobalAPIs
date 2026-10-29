using System.ComponentModel.DataAnnotations;

namespace DbDemoProject.Model.Data.Model
{
    public class LetterModel
    {
        [Key]
        public Guid? LetterUUID { get; set; }
        public string DocumentName { get; set; }
        public string DocumentType { get; set; }
        public byte[] FileByte { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
