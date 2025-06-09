using Microsoft.AspNetCore.Mvc;

namespace MVC_PrintSystem.Services
{
    public interface ILoginService
    {
        Task<IActionResult> Login(int cardId, HttpContext httpContext);
        IActionResult Logout(HttpContext httpContext);
    }

}