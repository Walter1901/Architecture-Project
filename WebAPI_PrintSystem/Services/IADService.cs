namespace WebAPI_PrintSystem.Services
{
    public interface IADService
    {
        Task<bool> AuthenticateAsync(string username, string password);
    }
}