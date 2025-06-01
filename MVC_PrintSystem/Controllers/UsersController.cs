using Microsoft.AspNetCore.Mvc;
using MVC_PrintSystem.Services;

namespace MVC_PrintSystem.Controllers
{
    public class UsersController : Controller
    {
        private readonly IWebAPIService _webAPIService;

        public UsersController(IWebAPIService webAPIService)
        {
            _webAPIService = webAPIService;
        }

        public IActionResult GetUsername() => View();

        [HttpPost]
        public async Task<IActionResult> GetUsername(string uid)
        {
            if (string.IsNullOrEmpty(uid))
            {
                ModelState.AddModelError("", "UID is required");
                return View();
            }

            var username = await _webAPIService.GetUsernameAsync(uid);
            ViewBag.Username = username;
            return View();
        }

        public async Task<IActionResult> FacultyStudents(string faculty)
        {
            var students = await _webAPIService.GetFacultyStudentsAsync(faculty);
            return View(students);
        }
    }
}