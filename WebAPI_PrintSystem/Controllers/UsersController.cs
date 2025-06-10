using Microsoft.AspNetCore.Mvc;
using WebAPI_PrintSystem.Services;
using PrintSystem.Models;

namespace WebAPI_PrintSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly PrintSystem.Models.Interfaces.ISAPHRService _sapHRService;
        private readonly ISqlService _sqlService;

        public UsersController(PrintSystem.Models.Interfaces.ISAPHRService sapHRService, ISqlService sqlService)
        {
            _sapHRService = sapHRService;
            _sqlService = sqlService;
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
                var students = new List<User>();

                var studentUsernames = new[] { "joaquim.jonathan", "student2", "marie.dupont" };

                foreach (var username in studentUsernames)
                {
                    var realQuota = await _sqlService.GetAvailableAmountAsync(username);

                    students.Add(new User
                    {
                        Username = username,
                        Faculty = "Computer Science",
                        AvailableQuota = realQuota,
                        Role = "Student",
                        LastUpdated = DateTime.Now
                    });
                }

                return Ok(students);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = ex.Message });
            }
        }

        [HttpGet("details/{username}")]
        public async Task<IActionResult> GetUserDetails(string username)
        {
            try
            {
                var realQuota = await _sqlService.GetAvailableAmountAsync(username);

                var user = new User
                {
                    Username = username,
                    Faculty = "Computer Science",
                    AvailableQuota = realQuota,
                    Role = username.Contains("faculty") ? "Faculty" : "Student"
                };

                return Ok(user);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = ex.Message });
            }
        }
    }
}