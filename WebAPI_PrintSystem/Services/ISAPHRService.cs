namespace WebAPI_PrintSystem.Services
{
    public interface ISAPHRService
    {
        Task<string> GetUsernameAsync(string uid);
    }
}