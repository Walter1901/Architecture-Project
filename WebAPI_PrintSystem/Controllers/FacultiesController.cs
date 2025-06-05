using Microsoft.AspNetCore.Mvc;
using WebAPI_PrintSystem.Services;
using PrintSystem.Models;

namespace WebAPI_PrintSystem.Controllers
{
    // API Controller for faculty management operations
    // Handles student list retrieval, quota allocation, and faculty statistics
    [ApiController]
    [Route("api/[controller]")]
    public class FacultiesController : ControllerBase
    {
        private readonly ISqlService _sqlService;
        private readonly ISAPHRService _sapHRService;

        // Constructor with dependency injection
        public FacultiesController(ISqlService sqlService, ISAPHRService sapHRService)
        {
            _sqlService = sqlService;
            _sapHRService = sapHRService;
        }

        // Retrieves list of students belonging to a specific faculty
        // Combines student data from HR system with current quota information
        [HttpGet("students/{faculty}")]
        public async Task<IActionResult> GetFacultyStudents(string faculty)
        {
            try
            {
                // In production, this would:
                // 1. Query SAP HR for students in faculty
                // 2. Get current quota for each student from database
                // 3. Combine data and return

                // Mock data for demonstration - replace with real implementation
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

                // Filter students by faculty (in production, this filtering would be in the database query)
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

        // Allocates printing quota to a specific student
        // Used by faculty members to add credit to student accounts
        [HttpPost("allocate-quota")]
        public async Task<IActionResult> AllocateQuota([FromBody] FacultyQuotaRequest request)
        {
            try
            {
                // Validate required fields
                if (string.IsNullOrEmpty(request.Username) || request.Amount <= 0)
                {
                    return BadRequest(new ApiResponse
                    {
                        Success = false,
                        ErrorMessage = "Username and amount are required. Amount must be greater than 0 CHF."
                    });
                }

                // Validate allocation limit (max 50 CHF per allocation for control)
                if (request.Amount > 50)
                {
                    return BadRequest(new ApiResponse
                    {
                        Success = false,
                        ErrorMessage = "Maximum allocation: 50 CHF per student."
                    });
                }

                // Add quota to student account
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

        // Generates quota summary statistics for a faculty
        // Provides overview of total allocations, average per student, etc.
        [HttpGet("quota-summary/{faculty}")]
        public async Task<IActionResult> GetQuotaSummary(string faculty)
        {
            try
            {
                // In production, calculate real statistics from database
                var summary = new
                {
                    Faculty = faculty,
                    TotalStudents = 3, // Count from database
                    TotalQuotaAllocated = 65.5f, // Sum from database in CHF
                    AverageQuotaPerStudent = 21.8f, // Calculated average in CHF
                    StudentsWithLowQuota = 1, // Count students with < 10 CHF
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
