using Microsoft.AspNetCore.Mvc;
using WebAPI_PrintSystem.Services;
using WebAPI_PrintSystem.Models;
using PrintSystem.Models;

namespace WebAPI_PrintSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentController : ControllerBase
    {
        private readonly ISqlService _sqlService;
        private readonly IPaymentDBService _paymentDBService;

        public PaymentController(ISqlService sqlService, IPaymentDBService paymentDBService)
        {
            _sqlService = sqlService;
            _paymentDBService = paymentDBService;
        }

        [HttpPost("online")]
        public async Task<IActionResult> ProcessOnlinePayment([FromBody] OnlinePaymentRequest request)
        {
            try
            {
                if (string.IsNullOrEmpty(request.Username) || request.Amount <= 0)
                {
                    return BadRequest(new ApiResponse { Success = false, ErrorMessage = "Invalid request data" });
                }

                var paymentSuccess = await _paymentDBService.TransferMoneyAsync(request.Username, request.Amount);

                if (paymentSuccess)
                {
                    var quotaAdded = await _sqlService.AddAmountAsync(request.Username, request.Amount);

                    if (quotaAdded)
                    {
                        return Ok(new ApiResponse { Success = true });
                    }
                }

                return BadRequest(new ApiResponse { Success = false, ErrorMessage = "Payment failed" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse { Success = false, ErrorMessage = ex.Message });
            }
        }
    }
}