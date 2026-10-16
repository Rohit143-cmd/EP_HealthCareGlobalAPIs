namespace DbDemoProject.Model
{
    public class RegisterUserRequest
    {
        public Guid? UserUUID { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Firstname { get; set; } = string.Empty;
        public string Lastname { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phonenumeber { get; set; } = string.Empty;
        public Guid? RoleUUID { get; set; }
    }
}
