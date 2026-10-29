using DbDemoProject.Model.Data.Model;
using DbDemoProject.Model.ResponseDTO;
using DbDemoProject.Service.Admin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DbDemoProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LetterController : ControllerBase
    {
        private readonly ILetterService _letterService;
        public LetterController(ILetterService letterService)
        {
            _letterService = letterService;
        }
        [HttpPost("addLetter")]
        [Consumes("multipart/form-data")]
        public IActionResult SaveLetter([FromForm] LetterRequestResponse letterModel)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = _letterService.SaveLetter(letterModel);

            return Ok(new
            {
                success = result > 0,
                message = result > 0
                    ? "Letter saved successfully."
                    : "Failed to save letter."
            });
        }
        [HttpGet("getLetter")]
        public IActionResult GetLetter()
        {
            var result = _letterService.GetLetter();
            return Ok(result);

        }

        [HttpGet("generateAppointmentLetter")]
        public async Task<IActionResult> GenerateAppointmentLetter(
            [FromQuery] Guid appointmentUUID)
        {
            if (appointmentUUID == Guid.Empty)
            {
                return BadRequest("A valid appointment UUID is required.");
            }

            try
            {
                byte[] pdfBytes =
                    await _letterService.GenerateAppointmentLetter(appointmentUUID);

                if (pdfBytes == null ||
                    pdfBytes.Length < 5 ||
                    System.Text.Encoding.ASCII.GetString(pdfBytes, 0, 5) != "%PDF-")
                {
                    return StatusCode(
                        StatusCodes.Status500InternalServerError,
                        "The generated file is not a valid PDF.");
                }

                return File(
                    pdfBytes,
                    "application/pdf",
                    $"AppointmentLetter_{appointmentUUID:N}.pdf");
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                // Log the exception on the server.
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    "Unable to generate the appointment letter.");
            }
        }

    }
}
