using DbDemoProject.Model;
using DbDemoProject.Model.ResponseDTO;

namespace DbDemoProject.Service
{
    public interface IAccountService
    {
        public Task<List<SystemUserResponseDto>> GetActiveUsers();
        public List<Users> GetDisabledUsers();
        public Users? GetUserByID(Guid? userid);
        public int DeleteUser(Guid? userid);
        public LoginResponseDto Login(LoginRequest loginRequest);
        public int RegisterUser(RegisterUserRequest user);
        public int UpdateUser(RegisterUserRequest user);

    }
}
