namespace WebAPI_PrintSystem.Services
{
    public interface ISqlService
    {
        Task<bool> AddAmountAsync(string username, float quotas);
        Task<float> GetAvailableAmountAsync(string username);
        Task<bool> DeductAmountAsync(string username, float amount);
    }
}