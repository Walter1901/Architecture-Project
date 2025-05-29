using System.Threading.Tasks;

namespace PrintSystem.Models.Interfaces
{
    public interface IADService
    {
        Task<bool> AuthenticateAsync(string username, string password);
    }
}