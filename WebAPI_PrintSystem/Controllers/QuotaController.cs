using Microsoft.AspNetCore.Mvc;
using WebAPI_PrintSystem.Services;
using WebAPI_PrintSystem.Models;

namespace WebAPI_PrintSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class QuotaController : ControllerBase
    {
        private readonly ISqlService _sqlService;

        public QuotaController(ISqlService sqlService)
        {
            _sqlService = sqlService;
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddAmount([FromBody] AddAmountRequest request)
        {
            try
            {
                if (string.IsNullOrEmpty(request.Username) || request.Quotas <= 0)
                {
                    return BadRequest(new PrintSystem.Models.ApiResponse
                    {
                        Success = false,
                        ErrorMessage = "Invalid request data"
                    });
                }

                var result = await _sqlService.AddAmountAsync(request.Username, request.Quotas);

                if (result)
                {
                    return Ok(new PrintSystem.Models.ApiResponse { Success = true });
                }

                return BadRequest(new PrintSystem.Models.ApiResponse
                {
                    Success = false,
                    ErrorMessage = "Failed to add quota"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new PrintSystem.Models.ApiResponse
                {
                    Success = false,
                    ErrorMessage = ex.Message
                });
            }
        }

        [HttpGet("available/{username}")]
        public async Task<IActionResult> GetAvailableAmount(string username)
        {
            try
            {
                if (string.IsNullOrEmpty(username))
                {
                    return BadRequest("Username is required");
                }

                var amount = await _sqlService.GetAvailableAmountAsync(username);
                return Ok(amount);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = ex.Message });
            }
        }
    }
}