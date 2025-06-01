using Microsoft.AspNetCore.Mvc;
using MVC_PrintSystem.Models.ViewModels;
using MVC_PrintSystem.Services;

namespace MVC_PrintSystem.Controllers
{
    public class LoginController : Controller
    {
        private readonly ILoginService _loginService;

        public LoginController(ILoginService loginService)
        {
            _loginService = loginService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await _loginService.Login(model.CardId);

            if (result is RedirectToActionResult redirectResult)
            {
                return redirectResult;
            }

            ModelState.AddModelError("", "Invalid login attempt.");
            return View(model);
        }

        public IActionResult Logout()
        {
            return _loginService.Logout();
        }
    }
}
