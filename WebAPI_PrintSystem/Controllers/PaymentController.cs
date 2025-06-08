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
                if (request == null || string.IsNullOrEmpty(request.Username) || request.Amount <= 0)
                {
                    return BadRequest(new PrintSystem.Models.ApiResponse
                    {
                        Success = false,
                        ErrorMessage = "Username and amount are required. Amount must be greater than 0 CHF."
                    });
                }

                if (request.Amount < 5 || request.Amount > 100)
                {
                    return BadRequest(new PrintSystem.Models.ApiResponse
                    {
                        Success = false,
                        ErrorMessage = "Amount must be between 5 and 100 CHF."
                    });
                }

                var transactionId = $"TXN_{DateTime.Now:yyyyMMddHHmmss}_{Guid.NewGuid().ToString("N")[..8]}";

                var paymentSuccess = await _paymentDBService.TransferMoneyAsync(request.Username, request.Amount);

                if (paymentSuccess)
                {
                    var quotaAdded = await _sqlService.AddAmountAsync(request.Username, request.Amount);

                    if (quotaAdded)
                    {
                        return Ok(new PrintSystem.Models.ApiResponse
                        {
                            Success = true,
                            Data = new
                            {
                                TransactionId = transactionId,
                                Amount = request.Amount,
                                Currency = "CHF",
                                NewBalance = await _sqlService.GetAvailableAmountAsync(request.Username)
                            }
                        });
                    }
                }

                return BadRequest(new PrintSystem.Models.ApiResponse
                {
                    Success = false,
                    ErrorMessage = "Payment processing failed. Please try again."
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new PrintSystem.Models.ApiResponse
                {
                    Success = false,
                    ErrorMessage = $"Server error: {ex.Message}"
                });
            }
        }

        [HttpGet("history/{username}")]
        public async Task<IActionResult> GetPaymentHistory(string username)
        {
            try
            {
                var history = new List<PrintSystem.Models.PaymentTransaction>
                {
                    new PrintSystem.Models.PaymentTransaction
                    {
                        Username = username,
                        Amount = 20.0f,
                        TransactionType = "TopUp",
                        TransactionDate = DateTime.Now.AddDays(-5),
                        Reference = "TXN_20241201_ABC123"
                    },
                    new PrintSystem.Models.PaymentTransaction
                    {
                        Username = username,
                        Amount = -2.40f,
                        TransactionType = "Print",
                        TransactionDate = DateTime.Now.AddDays(-2),
                        Reference = "PRINT_20241204_DEF456"
                    }
                };

                return Ok(history);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = ex.Message });
            }
        }
    }
}