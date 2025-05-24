using Microsoft.AspNetCore.Mvc;
using WebAPI_PrintSystem.Services;
using PrintSystem.Models;

namespace WebAPI_PrintSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly ISAPHRService _sapHRService;

        public UsersController(ISAPHRService sapHRService)
        {
            _sapHRService = sapHRService;
        }

        [HttpGet("username/{uid}")]
        public async Task<IActionResult> GetUsername(string uid)
        {
            try
            {
                if (string.IsNullOrEmpty(uid))
                {
                    return BadRequest("UID is required");
                }

                var username = await _sapHRService.GetUsernameAsync(uid);

                if (!string.IsNullOrEmpty(username))
                {
                    return Ok(new { Username = username });
                }

                return NotFound(new { Error = "User not found" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = ex.Message });
            }
        }

        [HttpGet("faculty/{faculty}")]
        public async Task<IActionResult> GetFacultyStudents(string faculty)
        {
            try
            {
                // Mock data for testing
                var students = new List<User>
                {
                    new User { Username = "joaquim.jonathan", Faculty = faculty, AvailableQuota = 75.0f },
                    new User { Username = "student2", Faculty = faculty, AvailableQuota = 50.0f }
                };

                return Ok(students);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = ex.Message });
            }
        }
    }
}