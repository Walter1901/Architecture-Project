using PrintSystem.Models;
using PrintSystem.Models.Interfaces;
using PrintSystem.BLL.Interfaces;
using System.Threading.Tasks;

namespace PrintSystem.BLL.Services
{
    public class UserService : IUserService
    {
        private readonly IADService _adService;
        private readonly ISAPHRService _sapHRService;

        public UserService(IADService adService, ISAPHRService sapHRService)
        {
            _adService = adService;
            _sapHRService = sapHRService;
        }

        public async Task<bool> AuthenticateUserAsync(string username, string password)
        {
            try
            {
                // Business validation
                if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                {
                    return false;
                }

                // REAL call to your existing AD service
                return await _adService.AuthenticateAsync(username, password);
            }
            catch
            {
                return false;
            }
        }

        public async Task<List<User>> GetFacultyStudentsAsync(string faculty)
        {
            try
            {
                // Business validation  
                if (string.IsNullOrEmpty(faculty))
                {
                    return new List<User>();
                }

                // Business logic: Get students for faculty
                // This could be enhanced to call a real repository
                var students = new List<User>
                {
                    new User { Username = "joaquim.jonathan", Faculty = faculty, AvailableQuota = 75.0f },
                    new User { Username = "student2", Faculty = faculty, AvailableQuota = 50.0f }
                };

                return students;
            }
            catch
            {
                return new List<User>();
            }
        }

        public async Task<string> GetUsernameFromUidAsync(string uid)
        {
            try
            {
                // Business validation
                if (string.IsNullOrEmpty(uid))
                {
                    return string.Empty;
                }

                // REAL call to your existing SAP service
                return await _sapHRService.GetUsernameAsync(uid);
            }
            catch
            {
                return string.Empty;
            }
        }
    }
}