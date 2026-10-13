using Azure.Core;
using DbDemoProject.Data;
using DbDemoProject.Helper;
using DbDemoProject.Model;
using Microsoft.AspNetCore.Identity;
using System.IdentityModel.Tokens.Jwt;

namespace DbDemoProject.Service
{
    public class AccountService : IAccountService
    {
        private readonly DbContextHelper _dbContextHelper;
        private readonly IPasswordHasher<Users> _passwordHasher;
        private readonly JWTHelper _jwtService;

        public AccountService(DbContextHelper dbContextHelper, IPasswordHasher<Users> passwordHasher, JWTHelper jWTHelper) 
        {
            _dbContextHelper = dbContextHelper;
            _passwordHasher = passwordHasher;
            _jwtService = jWTHelper;
        }

        public int DeleteUser(Guid? userid)
        {
            if (userid != null)
            {
                var user = _dbContextHelper.Users.Where(a=>a.UserUUID==userid).FirstOrDefault();
                if (user != null)
                {
                    user.IsActive = false;
                    _dbContextHelper.Users.Update(user);
                    return _dbContextHelper.SaveChanges();
                }
            }
            return 0;   
        }

        public List<Users> GetActiveUsers()
        {
            var result = _dbContextHelper.Users.Where(a=>a.IsActive==true).ToList();
            return result;
        }

        public List<Users> GetDisabledUsers()
        {
            var result = _dbContextHelper.Users.Where(a => a.IsActive == false).ToList();
            return result;
        }

        public Users? GetUserByID(Guid? userid)
        {
            if (userid != null)
            {
                return _dbContextHelper.Users.Where(a => a.UserUUID == userid).FirstOrDefault();
            }
            return null;
        }

        public LoginResponseDto? Login(LoginRequest loginRequest)
        {
            var user = _dbContextHelper.Users.Where(a => a.UserName == loginRequest.UserName && a.IsActive==true).FirstOrDefault();
            if (user!=null)
            {
                var passwordResult = _passwordHasher.VerifyHashedPassword(
                    user,
                    user.PasswordHash,
                    loginRequest.Password
                    );
                if(passwordResult == PasswordVerificationResult.Failed)
                {
                    return new LoginResponseDto
                    {
                        IsSuccess = false,
                        Message = "Username or Password Invalid..."
                    };
                }
                var token = _jwtService.GenerateToken(user);
                var userrole = _dbContextHelper.Roles.Where(role => role.RoleUUID == user.RoleUUID).FirstOrDefault();
                var product = _dbContextHelper.Product.Where(p => p.ProductUUID == user.ProductUUID).FirstOrDefault();
                if(userrole!=null && product != null)
                {
                    user.Role = userrole;
                    user.Product = product;
                }else
                    return new LoginResponseDto{
                        IsSuccess = false,
                        Message = "Product or Role not found..."
                    };
                user.PasswordHash = null;
                return new LoginResponseDto
                {
                    user = user,
                    Token = token,
                    IsSuccess = true,
                    Message = "Success"
                };
            }
            return new LoginResponseDto
            {
                IsSuccess = false,
                Message = "Username or Password Invalid..."
            };
        }

        public int RegisterUser(RegisterUserRequest usersIncoming)
        {
            Users user = new Users();
            if (user != null)
            {
                var IsExists = _dbContextHelper.Users.Any(_a => _a.UserName == usersIncoming.UserName);
                if (IsExists)
                    return 2;
                user.UserUUID = Guid.NewGuid();
                user.IsActive = true;
                user.EmailID = usersIncoming.Email;
                user.UserName = usersIncoming.UserName;
                user.PhoneNumber = usersIncoming.Phonenumeber;
                // Hash password
                user.PasswordHash = _passwordHasher.HashPassword(
                    user,
                    usersIncoming.Password
                );
                _dbContextHelper.Users.Add(user);
                var result = _dbContextHelper.SaveChanges();
                return result;
            }
            return 0;
        }

        public int UpdateUser(Users user)
        {
            if(user!=null && user.UserUUID != null)
            {
                _dbContextHelper.Users.Update(user);
                return _dbContextHelper.SaveChanges();
            }
            return 0;
        }
    }
}
