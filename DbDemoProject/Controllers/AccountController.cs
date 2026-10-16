using DbDemoProject.Data;
using DbDemoProject.Helper;
using DbDemoProject.Model;
using DbDemoProject.Service;
using DbDemoProject.Service.Account;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;

namespace DbDemoProject.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly INavigationMenuService _navigationMenuService;
        private readonly IDashbordItems _dashbordItems;
        public AccountController(IAccountService account, INavigationMenuService navigationMenuService, IDashbordItems dashbordItems)
        {
            _accountService = account;
            _navigationMenuService = navigationMenuService;
            _dashbordItems = dashbordItems;
        }
        [Authorize]
        [HttpGet("getActiveUsers")]
        public async Task<IActionResult> GetActiveUsers()
        {
            try
            {
                var result = await _accountService.GetActiveUsers();
                return Ok(result);
            }
            catch(Exception ex)
            {
                throw ex;
            }
            
        }
        [Authorize]
        [HttpGet("getDisabledUsers")]
        public IActionResult GetDisabledUsers()
        {
            var result = _accountService.GetDisabledUsers();
            return Ok(result);
        }
        [Authorize]
        [HttpGet("getUserById/Userid")]
        public IActionResult GetUserByID(Guid? Id)
        {
            var result = _accountService.GetUserByID(Id);
            if (result != null)
                return Ok(result);
            return BadRequest();
        }
        [Authorize]
        [HttpDelete("deleteUser/{userUUID}")]
        public IActionResult DeleteUser(Guid? userUUID)
        {
            var result = _accountService.DeleteUser(userUUID);
            if (result == 1)
                return Ok(new ApiResponseDto
                {
                    Message = "User deleted...",
                    Success = true
                });
            return BadRequest();
        }
        [HttpPost("registerUser")]
        public IActionResult RegisterUser(RegisterUserRequest user)
        {
            var result = _accountService.RegisterUser(user);
            if (result == 1)
            {
                return Ok(new ApiResponseDto
                {
                    Message = "Registration Successful...",
                    Success = true
                });
            }
            else if (result == 2)
            {
                return BadRequest(new ApiResponseDto
                {
                    Message = "Email Id already exists please enter different email...",
                    Success = false

                });
            }
            else
            {
                return BadRequest(new ApiResponseDto
                {
                    Message = "Bad request, please try after some time...",
                    Success = false

                });
            }
        }
        [HttpPut("updateUser")]
        [Authorize]
        public IActionResult UpdateUser(RegisterUserRequest user)
        {
            var result = _accountService.UpdateUser(user);
            if (result == 1)
            {
                return Ok(new ApiResponseDto
                {
                    Message = "User data updated",
                    Success = true
                });
            }
            else if (result == 0)
            {
                return BadRequest(new ApiResponseDto
                {
                    Message = "Bad requeset, please try again...",
                    Success = false

                });
            }
            else
            {
                return BadRequest(new ApiResponseDto
                {
                    Message = "Bad request, please try after some time...",
                    Success = false

                });
            }
        }
        [Authorize]
        [HttpGet("getProductUserDetails")]
        public IActionResult GetProductUserDetails([FromQuery] Guid? roleUUID)
        {
            var result = _navigationMenuService.GetActiveMenuByRoleID(roleUUID);

            return Ok(result);
        }

        //[Authorize]
        [HttpGet("getDashboard")]
        public async Task<IActionResult> GetDashboardItem([FromQuery] Guid? roleUUID)
        {
            var result = await _dashbordItems.GetDashboardItems(roleUUID);
            return Ok(result);
        }

        [HttpPost("login")]
        public IActionResult Login(LoginRequest loginRequest)
        {
            var result = _accountService.Login(loginRequest);
            if (result.IsSuccess)
                return Ok(new ApiResponseDto
                {
                    Message = result.Message,
                    Success = result.IsSuccess,
                    Data = result.user,
                    Tokan = result.Token
                });
            else
                return BadRequest(new ApiResponseDto
                {
                    Message = result.Message,
                    Success = result.IsSuccess
                });
        }
    }
}
