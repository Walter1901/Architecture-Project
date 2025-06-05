using Microsoft.AspNetCore.Mvc;

namespace MVC_PrintSystem.Controllers
{
    public abstract class BaseController : Controller
    {
       
        protected string? CurrentUsername => HttpContext.Session.GetString("Username");
        protected string? CurrentUserRole => HttpContext.Session.GetString("Role");
        protected string? CurrentUserFaculty => HttpContext.Session.GetString("Faculty");
        protected bool IsUserLoggedIn => HttpContext.Session.GetString("IsLoggedIn") == "true";

        
        protected bool CheckIfLoggedIn()
        {
            return IsUserLoggedIn;
        }

        
        protected bool CheckRole(string requiredRole)
        {
            return IsUserLoggedIn && CurrentUserRole == requiredRole;
        }

        // Méthode pour rediriger vers login avec message d'erreur
        protected IActionResult RedirectToLogin(string? error = null)
        {
            if (!string.IsNullOrEmpty(error))
            {
                return RedirectToAction("Index", "Login", new { error });
            }
            return RedirectToAction("Index", "Login");
        }
    }
}