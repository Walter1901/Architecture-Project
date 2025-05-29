using Microsoft.AspNetCore.Mvc;
using MVC_PrintSystem.Services;
using PrintSystem.Models;

namespace MVC_PrintSystem.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
