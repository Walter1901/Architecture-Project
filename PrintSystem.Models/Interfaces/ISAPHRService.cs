using System.Threading.Tasks;

namespace PrintSystem.Models.Interfaces
{
    public interface ISAPHRService
    {
        Task<string> GetUsernameAsync(string uid);
    }
}