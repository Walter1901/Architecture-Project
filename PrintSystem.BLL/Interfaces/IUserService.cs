using PrintSystem.Models;
using System.Threading.Tasks;

namespace PrintSystem.BLL.Interfaces
{
    public interface IUserService
    {
        Task<bool> AuthenticateUserAsync(string username, string password);
        Task<List<User>> GetFacultyStudentsAsync(string faculty);
        Task<string> GetUsernameFromUidAsync(string uid);
    }
}