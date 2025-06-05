using Microsoft.AspNetCore.Mvc;
using MVC_PrintSystem.Models.ViewModels;
using MVC_PrintSystem.Services;

namespace MVC_PrintSystem.Controllers
{
    public class LoginController : Controller

    {


        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Test simple pour vérifier que ça marche
            if (model.CardId == 123)
            {
                // Stocker en session
                HttpContext.Session.SetString("Username", "test.user");
                HttpContext.Session.SetString("Role", "Student");
                HttpContext.Session.SetString("IsLoggedIn", "true");

                return RedirectToAction("Dashboard", "Students");
            }

            ModelState.AddModelError("", "Card ID invalide. Essayez 123 pour tester.");
            return View(model);
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }
    }
}