using System.ComponentModel.DataAnnotations;

namespace DbDemoProject.Model.ResponseDTO
{
    public class DashboardItemResponseDto
    {
        public int id { get; set; }
        public int value { get; set; } = 0;
        public string icon { get; set; }
        public string title { get; set; }
        public string gradient { get; set; }
        public string shadowColor { get; set; }
        public string viewAllURL { get; set; }
        public string buttonLabel { get; set; }
    }
}
