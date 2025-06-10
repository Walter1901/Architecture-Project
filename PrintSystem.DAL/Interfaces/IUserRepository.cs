using PrintSystem.Models;

namespace PrintSystem.DAL.Interfaces
{
    public interface IUserRepository
    {
        Task<List<User>> GetFacultyStudentsAsync(string faculty);
        Task<User?> GetUserDetailsAsync(string username);
        Task<string> GetUsernameFromUidAsync(string uid);
    }
}