using System.ComponentModel.DataAnnotations;

namespace DbDemoProject.Model.ResponseDTO
{
    public class LetterRequestResponse
    {
        [Required]
        public string DocumentName { get; set; } = string.Empty;

        [Required]
        public string DocumentType { get; set; } = string.Empty;

        [Required]
        public IFormFile FileByte { get; set; } = null!;
    }
}
