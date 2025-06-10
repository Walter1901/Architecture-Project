using Microsoft.AspNetCore.Mvc;
using WebAPI_PrintSystem.Services;
using PrintSystem.Models;

namespace WebAPI_PrintSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FacultiesController : ControllerBase
    {
        private readonly ISqlService _sqlService;
        private readonly PrintSystem.Models.Interfaces.ISAPHRService _sapHRService;

        public FacultiesController(ISqlService sqlService, PrintSystem.Models.Interfaces.ISAPHRService sapHRService)
        {
            _sqlService = sqlService;
            _sapHRService = sapHRService;
        }

        [HttpGet("students/{faculty}")]
        public async Task<IActionResult> GetFacultyStudents(string faculty)
        {
            try
            {
                var students = new List<FacultyStudent>
                {
                    new FacultyStudent
                    {
                        Username = "joaquim.jonathan",
                        FirstName = "Joaquim",
                        LastName = "Jonathan",
                        Email = "joaquim.jonathan@hes-so.ch",
                        AvailableQuota = await _sqlService.GetAvailableAmountAsync("joaquim.jonathan"),
                        LastActivity = DateTime.Now.AddDays(-1)
                    },
                    new FacultyStudent
                    {
                        Username = "marie.dupont",
                        FirstName = "Marie",
                        LastName = "Dupont",
                        Email = "marie.dupont@hes-so.ch",
                        AvailableQuota = await _sqlService.GetAvailableAmountAsync("marie.dupont"),
                        LastActivity = DateTime.Now.AddDays(-3)
                    },
                    new FacultyStudent
                    {
                        Username = "paul.martin",
                        FirstName = "Paul",
                        LastName = "Martin",
                        Email = "paul.martin@hes-so.ch",
                        AvailableQuota = await _sqlService.GetAvailableAmountAsync("paul.martin"),
                        LastActivity = DateTime.Now.AddDays(-7)
                    }
                };

                var filteredStudents = students.Where(s =>
                    faculty.ToLower() == "informatique" ||
                    faculty.ToLower() == "it" ||
                    faculty.ToLower() == "computer_science").ToList();

                return Ok(filteredStudents);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = ex.Message });
            }
        }

        [HttpPost("allocate-quota")]
        public async Task<IActionResult> AllocateQuota([FromBody] FacultyQuotaRequest request)
        {
            try
            {
                if (string.IsNullOrEmpty(request.Username) || request.Amount <= 0)
                {
                    return BadRequest(new ApiResponse
                    {
                        Success = false,
                        ErrorMessage = "Username and amount are required. Amount must be greater than 0 CHF."
                    });
                }

                if (request.Amount > 50)
                {
                    return BadRequest(new ApiResponse
                    {
                        Success = false,
                        ErrorMessage = "Maximum allocation: 50 CHF per student."
                    });
                }

                var result = await _sqlService.AddAmountAsync(request.Username, request.Amount);

                if (result)
                {
                    return Ok(new ApiResponse
                    {
                        Success = true,
                        Data = new
                        {
                            Username = request.Username,
                            AmountAllocated = request.Amount,
                            Currency = "CHF",
                            AllocatedBy = request.AllocatedBy,
                            NewBalance = await _sqlService.GetAvailableAmountAsync(request.Username),
                            Date = DateTime.Now,
                            Reason = request.Reason
                        }
                    });
                }

                return BadRequest(new ApiResponse
                {
                    Success = false,
                    ErrorMessage = "Failed to allocate quota."
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse
                {
                    Success = false,
                    ErrorMessage = ex.Message
                });
            }
        }

        [HttpGet("quota-summary/{faculty}")]
        public async Task<IActionResult> GetQuotaSummary(string faculty)
        {
            try
            {
                var summary = new
                {
                    Faculty = faculty,
                    TotalStudents = 3,
                    TotalQuotaAllocated = 65.5f,
                    AverageQuotaPerStudent = 21.8f,
                    StudentsWithLowQuota = 1,
                    Currency = "CHF",
                    LastUpdated = DateTime.Now
                };

                return Ok(summary);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = ex.Message });
            }
        }
    }
}