using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;


namespace MVC_PrintSystem.Services
{
    public class LoginService : ILoginService
    {
        private readonly IWebAPIService _webAPIService;

        public LoginService(IWebAPIService webAPIService)
        {
            _webAPIService = webAPIService;
        }

        public async Task<IActionResult> Login(int cardId, HttpContext httpContext)
        {
            try
            {
                var username = await _webAPIService.GetUsernameAsync(cardId.ToString());

                if (!string.IsNullOrEmpty(username))
                {
                    var user = await _webAPIService.GetUserDetailsAsync(username);

                    if (user != null)
                    {
                        
                        httpContext.Session.SetString("Username", user.Username);
                        httpContext.Session.SetString("Role", user.Role);
                        httpContext.Session.SetString("Faculty", user.Faculty ?? "");
                        httpContext.Session.SetString("IsLoggedIn", "true");

                        
                        if (user.Role == "Student")
                        {
                            return new RedirectToActionResult("Dashboard", "Students", null);
                        }
                        else if (user.Role == "Faculty")
                        {
                            return new RedirectToActionResult("Dashboard", "Faculties", null);
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

        public IActionResult Logout(HttpContext httpContext)
        {
            
            httpContext.Session.Clear();
            return new RedirectToActionResult("Index", "Login", null);
        }

    }

}