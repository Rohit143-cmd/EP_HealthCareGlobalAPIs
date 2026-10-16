using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DbDemoProject.Model.Data.Model
{
    public class Dashboard
    {
        [Key]
        public Guid? DashbordUUID { get; set; }
        public Guid? RoleUUID { get; set; }
        [ForeignKey(nameof(RoleUUID))]
        public UserRoles Role {  get; set; }
        public int id { get; set; }
        public int value { get; set; } = 0;
        public string icon { get; set; }
        public string title { get; set; }
        public string gradient { get; set; }
        public string shadowColor { get; set; }
        public string nameofTableCount { get; set; }
        public string viewAllURL { get; set; }
        public string buttonLabel { get; set; }
    }
}
