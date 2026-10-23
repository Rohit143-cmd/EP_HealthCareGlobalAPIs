using Azure.Core;
using DbDemoProject.Data;
using DbDemoProject.Helper;
using DbDemoProject.Model;
using DbDemoProject.Model.ResponseDTO;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
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
                var user = _dbContextHelper.Users.Where(a=>a.UserUUID == userid).FirstOrDefault();
                if (user != null)
                {
                    user.IsActive = false;
                    _dbContextHelper.Users.Update(user);
                    return _dbContextHelper.SaveChanges();
                }
            }
            return 0;   
        }

        public async Task<List<SystemUserResponseDto>> GetActiveUsers()
        {
            var result = await (
                from user in _dbContextHelper.Users

                join roleData in _dbContextHelper.Roles
                    on user.RoleUUID equals roleData.RoleUUID into roles
                from role in roles.DefaultIfEmpty()

                join productData in _dbContextHelper.Product
                    on user.ProductUUID equals productData.ProductUUID into products
                from product in products.DefaultIfEmpty()

                where user.IsActive == true

                select new SystemUserResponseDto
                {
                    UserUUID = user.UserUUID,
                    UserName = user.UserName,
                    FirstName = user.FirstName,
                    LastName = user.LastName,

                    RoleUUID = role != null ? role.RoleUUID : null,
                    Rolename = role != null ? role.RoleName : null,

                    ProductUUID = product != null ? product.ProductUUID : null,
                    ProductName = product != null ? product.ProductName : null,

                    IsActive = user.IsActive
                }
            ).ToListAsync();

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

        public async Task<List<SystemUserResponseDto>> GetUserByRole(string rolename)
        {
            var result = await (
                from user in _dbContextHelper.Users
                join role in _dbContextHelper.Roles
                    on user.RoleUUID equals role.RoleUUID
                where role.RoleName.ToLower() == rolename.ToLower()
                select new SystemUserResponseDto
                {
                    UserUUID = user.UserUUID,
                    UserName = user.UserName,
                    RoleUUID = role.RoleUUID,
                    Rolename = role.RoleName
                }
            ).ToListAsync();
            return result;
        }

        public LoginResponseDto? Login(LoginRequest loginRequest)
        {
            string token = null;
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
                
                var userrole = _dbContextHelper.Roles.Where(role => role.RoleUUID == user.RoleUUID).FirstOrDefault();
                var product = _dbContextHelper.Product.Where(p => p.ProductUUID == user.ProductUUID).FirstOrDefault();
                if(userrole!=null && product != null)
                {
                    user.Role = userrole;
                    user.Product = product;
                    token = _jwtService.GenerateToken(user);
                }
                else
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
                    Message = "success"
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
                user.FirstName = usersIncoming.Firstname;
                user.LastName = usersIncoming.Lastname;
                user.RoleUUID = usersIncoming.RoleUUID;
                user.ProductUUID = Guid.Parse("09934910-A531-440A-AF69-67D683621263");
                // Hash password
                //Termproray Password
                usersIncoming.Password = "12345";
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

        public int UpdateUser(RegisterUserRequest user)
        {
            if(user!=null && user.UserUUID != null)
            {
                var result = _dbContextHelper.Users.Where(a=>a.UserUUID==user.UserUUID).FirstOrDefault();
                if(result!=null)
                {
                    result.FirstName = user.Firstname;
                    result.LastName = user.Lastname;
                    result.UserName = user.UserName;
                    result.RoleUUID = user.RoleUUID;
                    _dbContextHelper.Users.Update(result);
                    return _dbContextHelper.SaveChanges();
                }
            }
            return 0;
        }
    }
}
