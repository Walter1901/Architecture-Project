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

            if (model.CardId == 123)
            {
                HttpContext.Session.SetString("Username", "test.user");
                HttpContext.Session.SetString("Role", "Student");
                HttpContext.Session.SetString("IsLoggedIn", "true");

                return RedirectToAction("Dashboard", "Students");
            }
            else if (model.CardId == 456)
            {
                HttpContext.Session.SetString("Username", "test.admin");
                HttpContext.Session.SetString("Role", "Faculty");
                HttpContext.Session.SetString("IsLoggedIn", "true");

                return RedirectToAction("Dashboard", "Faculty");
            }

            ModelState.AddModelError("", "Card ID invalide. Try 123 for Student or 456 for Faculty.");
            return View(model);
        }



        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }
    }
}