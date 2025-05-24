namespace WebAPI_PrintSystem.Services
{
    public interface IPaymentDBService
    {
        Task<bool> TransferMoneyAsync(string username, float amount);
    }
}