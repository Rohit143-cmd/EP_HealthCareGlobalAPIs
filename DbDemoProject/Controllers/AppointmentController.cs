using DbDemoProject.Model.Data.Model;
using DbDemoProject.Model.ResponseDTO;
using DbDemoProject.Service.Appointment;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DbDemoProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentController : ControllerBase
    {

        private readonly IAppointmentService _appointmentService;
        public AppointmentController(IAppointmentService appointmentService)
        {
            _appointmentService = appointmentService;
        }

        [HttpPost("bookAppointment")]
        public async Task<IActionResult> BookAppointment([FromBody] AppointmentRequestModel appointmentRequest)
        {
            var result = await _appointmentService.BookAppointment(appointmentRequest);
            return Ok();

        }
        [HttpGet("getAppointment")]
        public async Task<IActionResult> GetAppointment()
        {
            var result = await _appointmentService.GetAppointment();
            return Ok(result);
        }
        [HttpGet("getAppointmentByUser/{userUUID}")]
        public async Task<IActionResult> GetAppointmentByUser(Guid? userUUID)
        {
            var result = await _appointmentService.GetAppointmentByUser(userUUID);
            return Ok(result);
        }

    }
}
