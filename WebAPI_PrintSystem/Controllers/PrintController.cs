using Microsoft.AspNetCore.Mvc;
using WebAPI_PrintSystem.Services;
using WebAPI_PrintSystem.Models;

namespace WebAPI_PrintSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PrintController : ControllerBase
    {
        private readonly ISqlService _sqlService;
        private readonly IADService _adService;

        public PrintController(ISqlService sqlService, IADService adService)
        {
            _sqlService = sqlService;
            _adService = adService;
        }

        [HttpPost("check-available")]
        public async Task<IActionResult> GetAvailableAmount([FromBody] PrintCheckRequest request)
        {
            try
            {
                if (string.IsNullOrEmpty(request.Username) || string.IsNullOrEmpty(request.Password))
                {
                    return BadRequest("Username and password are required");
                }

                var isAuthenticated = await _adService.AuthenticateAsync(request.Username, request.Password);

                if (!isAuthenticated)
                {
                    return Unauthorized(new { Error = "Authentication failed" });
                }

                var availableAmount = await _sqlService.GetAvailableAmountAsync(request.Username);

                return Ok(new { AvailableAmount = availableAmount });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = ex.Message });
            }
        }

        [HttpPost("deduct")]
        public async Task<IActionResult> DeductAmount([FromBody] PrintDeductRequest request)
        {
            try
            {
                if (string.IsNullOrEmpty(request.Username) || string.IsNullOrEmpty(request.Password) || request.Amount <= 0)
                {
                    return BadRequest("Invalid request data");
                }

                var isAuthenticated = await _adService.AuthenticateAsync(request.Username, request.Password);
                if (!isAuthenticated)
                    return Unauthorized();

                var result = await _sqlService.DeductAmountAsync(request.Username, request.Amount);

                if (result)
                {
                    return Ok(new { Success = true });
                }

                return BadRequest(new { Error = "Insufficient credit or deduction failed" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = ex.Message });
            }
        }
    }
}