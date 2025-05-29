using PrintSystem.Models;
using PrintSystem.BLL.Interfaces;
using PrintSystem.DAL.Interfaces;
using System.Threading.Tasks;

namespace PrintSystem.BLL.Services
{
    public class QuotaService : IQuotaService
    {
        private readonly IQuotaRepository _quotaRepository;

        public QuotaService(IQuotaRepository quotaRepository)
        {
            _quotaRepository = quotaRepository;
        }

        public async Task<ApiResponse> AddQuotaAsync(string username, float amount)
        {
            try
            {
                // Business validation
                if (string.IsNullOrEmpty(username) || amount <= 0)
                {
                    return new ApiResponse { Success = false, ErrorMessage = "Invalid username or amount" };
                }

                // REAL call to DAL
                var result = await _quotaRepository.AddAmountAsync(username, amount);

                return new ApiResponse { Success = result };
            }
            catch (Exception ex)
            {
                return new ApiResponse { Success = false, ErrorMessage = ex.Message };
            }
        }

        public async Task<float> GetAvailableQuotaAsync(string username)
        {
            try
            {
                // REAL call to DAL
                return await _quotaRepository.GetAvailableAmountAsync(username);
            }
            catch
            {
                return 0f;
            }
        }

        public async Task<ApiResponse> DeductQuotaAsync(string username, float amount)
        {
            try
            {
                if (amount <= 0)
                {
                    return new ApiResponse { Success = false, ErrorMessage = "Amount must be positive" };
                }

                // REAL call to DAL
                var result = await _quotaRepository.DeductAmountAsync(username, amount);

                return new ApiResponse { Success = result };
            }
            catch (Exception ex)
            {
                return new ApiResponse { Success = false, ErrorMessage = ex.Message };
            }
        }
    }
}