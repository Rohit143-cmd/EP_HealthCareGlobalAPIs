using DbDemoProject.Model;
using DbDemoProject.Service.Account;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DbDemoProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _roleService;
        public RoleController(IRoleService roleService) 
        {
            _roleService = roleService;
        }

        [HttpGet("getActiveRole")]
        public IActionResult GetActiveRole()
        {
            var role = _roleService.GetActiveRole();
            if(role!=null)
                return Ok(role);
            else return BadRequest();
        }
        [HttpPost("createRole")]
        public IActionResult CreateRole(UserRoles userRoles)
        {
            var result = _roleService.CreateRole(userRoles);
            if (result == 1)
                return Ok();
            else return BadRequest();
        }
        [HttpPut("updateRole")]
        public IActionResult UpdateRole(UserRoles userRoles)
        {
            var result = _roleService.UpdateRole(userRoles);
            if (result == 1)
                return Ok();
            else return BadRequest();
        }
        [HttpGet("getRoleById/{roleUUID}")]
        public IActionResult GetRoleByID(Guid? roleUUID)
        {
            var result = _roleService.GetRoleByID(roleUUID);
            if (result != null)
                return Ok(result);
            else return BadRequest();
        }
        [HttpDelete("deleteRoleById/{roleUUID}")]
        public IActionResult DeleteRoleByID(Guid? roleUUID)
        {
            var result = _roleService.DeleteRoleByID(roleUUID);
            if (result == 1)
                return Ok();
            else return BadRequest();
        }
    }
}
