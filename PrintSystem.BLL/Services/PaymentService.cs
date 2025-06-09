using PrintSystem.Models;
using PrintSystem.BLL.Interfaces;
using PrintSystem.DAL.Interfaces;
namespace PrintSystem.BLL.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly IQuotaRepository _quotaRepository;

        public PaymentService(IPaymentRepository paymentRepository, IQuotaRepository quotaRepository)
        {
            _paymentRepository = paymentRepository;
            _quotaRepository = quotaRepository;
        }

        public async Task<ApiResponse> ProcessOnlinePaymentAsync(string username, float amount)
        {
            try
            {
                // Business validation
                if (string.IsNullOrEmpty(username))
                {
                    return new ApiResponse { Success = false, ErrorMessage = "Username is required" };
                }

                if (amount <= 0)
                {
                    return new ApiResponse { Success = false, ErrorMessage = "Amount must be positive" };
                }

                if (amount > 500)
                {
                    return new ApiResponse { Success = false, ErrorMessage = "Payment amount exceeds maximum limit of 500 CHF" };
                }

                // REAL call to DAL to record the payment
                var paymentResult = await _paymentRepository.TransferMoneyAsync(username, amount);

                if (paymentResult)
                {
                    // REAL call to add quota after payment
                    var quotaResult = await _quotaRepository.AddAmountAsync(username, amount);

                    if (quotaResult)
                    {
                        return new ApiResponse { Success = true, Data = $"Payment of {amount} CHF processed and quota added for {username}" };
                    }
                }

                return new ApiResponse { Success = false, ErrorMessage = "Payment processing failed" };
            }
            catch (Exception ex)
            {
                return new ApiResponse { Success = false, ErrorMessage = $"Payment error: {ex.Message}" };
            }
        }

        public async Task<List<PaymentTransaction>> GetPaymentHistoryAsync(string username)
        {
            try
            {
                // REAL call to repository
                return await _paymentRepository.GetPaymentHistoryAsync(username);
            }
            catch
            {
                return new List<PaymentTransaction>();
            }
        }
    }
}