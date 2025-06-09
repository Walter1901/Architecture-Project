using PrintSystem.Models;

namespace MVC_PrintSystem.Services
{
    public interface IWebAPIService
    {
        Task<ApiResponse> AddAmountAsync(string username, float quotas);
        Task<float> GetAvailableAmountAsync(string username);
        Task<ApiResponse> ProcessOnlinePaymentAsync(string username, float amount);
        Task<List<User>> GetFacultyStudentsAsync(string faculty);
        Task<ApiResponse> AddFacultyAsync(string facultyName);
        Task<string> GetUsernameAsync(string uid);
        Task<User> GetUserDetailsAsync(string username);

        
        Task<List<FacultyStudent>> GetFacultyStudentsDetailedAsync(string faculty);
        Task<ApiResponse> AllocateFacultyQuotaAsync(string username, float amount, string allocatedBy, string reason);
        Task<List<PaymentTransaction>> GetPaymentHistoryAsync(string username);
        Task<object> GetFacultyQuotaSummaryAsync(string faculty);
    }
}
