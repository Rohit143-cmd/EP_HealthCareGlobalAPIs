namespace DbDemoProject.Model.ResponseDTO
{
    public class SystemUserResponseDto
    {
        public Guid? UserUUID { get; set; }
        public string? UserName { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }

        public Guid? RoleUUID { get; set; }
        public string? Rolename { get; set; }

        public Guid? ProductUUID { get; set; }
        public string? ProductName { get; set; }

        public bool IsActive { get; set; }
    }
}
