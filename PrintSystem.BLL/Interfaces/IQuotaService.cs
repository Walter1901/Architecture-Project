using PrintSystem.Models;
using System.Threading.Tasks;

namespace PrintSystem.BLL.Interfaces
{
    public interface IQuotaService
    {
        Task<ApiResponse> AddQuotaAsync(string username, float amount);
        Task<float> GetAvailableQuotaAsync(string username);
        Task<ApiResponse> DeductQuotaAsync(string username, float amount);
    }
}