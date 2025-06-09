using PrintSystem.Models;
using System.Threading.Tasks;

namespace PrintSystem.BLL.Interfaces
{
    public interface IPaymentService
    {
        Task<ApiResponse> ProcessOnlinePaymentAsync(string username, float amount);
        Task<List<PaymentTransaction>> GetPaymentHistoryAsync(string username);
    }
}