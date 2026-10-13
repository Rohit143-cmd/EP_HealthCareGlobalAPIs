namespace DbDemoProject.Model
{
    public class LoginResponseDto
    {
        public Users? user { get; set; }
        //public UserRoles? role { get; set; }
        //public Product? product { get; set; }
        public string? Token { get; set; }
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
    }
}
