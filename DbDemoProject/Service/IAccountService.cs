using DbDemoProject.Model;

namespace DbDemoProject.Service
{
    public interface IAccountService
    {
        public List<Users> GetActiveUsers();
        public List<Users> GetDisabledUsers();
        public Users? GetUserByID(Guid? userid);
        public int DeleteUser(Guid? userid);
        public LoginResponseDto Login(LoginRequest loginRequest);
        public int RegisterUser(RegisterUserRequest user);
        public int UpdateUser(Users user);

    }
}
