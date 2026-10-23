using DbDemoProject.Model.Data.Model;
using DbDemoProject.Service.Account;
using DbDemoProject.Service.Appointment;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DbDemoProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class MemberController : ControllerBase
    {
        private readonly IMemberService _memberService;
        public MemberController(IMemberService memberService)
        {
            _memberService = memberService;
        }
        [HttpGet("getMembers")]
        public async Task<IActionResult> GetMembers()
        {
            var result = await _memberService.GetMembers();
            return Ok(result);
        }
        [HttpPost("addMember")]
        public async Task<IActionResult> AddMember([FromBody] MemberModel memberModel)
        {
            var result = await _memberService.SaveMember(memberModel);
            return Ok();

        }
        [HttpPost("updateMember")]
        public async Task<IActionResult> UpdateMember([FromBody] MemberModel memberModel)
        {
            var result = await _memberService.UpdapteMember(memberModel);
            return Ok();

        }
        [HttpGet("getMemberById/{memberUUID}")]
        public IActionResult GetMemberById(Guid? memberUUID)
        {
            var result = _memberService.GetMemberById(memberUUID);
            return Ok();

        }
        [HttpGet("deleteMemberById/{memberUUID}")]
        public IActionResult DeleteMember(Guid? memberUUID)
        {
            var result = _memberService.DeleteMember(memberUUID);
            return Ok();

        }
    }
}
