using Microsoft.AspNetCore.Mvc;
using MVC_PrintSystem.Services;


namespace MVC_PrintSystem.Services
{
    public class LoginService : ILoginService
    {
        private readonly IWebAPIService _webAPIService;

        public LoginService(IWebAPIService webAPIService)
        {
            _webAPIService = webAPIService;
        }

        public async Task<IActionResult> Login(int cardId)
        {
            try
            {
                var username = await _webAPIService.GetUsernameAsync(cardId.ToString());

                if (!string.IsNullOrEmpty(username))
                {
                    var user = await _webAPIService.GetUserDetailsAsync(username);

                    if (user != null)
                    {
                        if (user.Role == "Student")
                        {
                            return new RedirectToActionResult("Dashboard", "Students", null);
                        }
                        else if (user.Role == "Faculty")
                        {
                            return new RedirectToActionResult("Index", "FacultyManagement", null);
                        }
                    }
                }

                return new RedirectToActionResult("Index", "Login", 
                    new { error = "Invalid Card ID or User not found" });
            }
            catch (Exception ex)
            {
                return new RedirectToActionResult("Index", "Login", 
                    new { error = $"Error: {ex.Message}" });
            }
        }

        public IActionResult Logout()
        {
            // Clear session or authentication cookies if needed
            return new RedirectToActionResult("Index", "Login", null);
        }
    }
}
