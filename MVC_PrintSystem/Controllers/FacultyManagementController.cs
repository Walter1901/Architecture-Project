using Microsoft.AspNetCore.Mvc;
using MVC_PrintSystem.Services;

namespace MVC_PrintSystem.Controllers
{
    public class FacultyManagementController : Controller
    {
        private readonly IWebAPIService _webAPIService;

        public FacultyManagementController(IWebAPIService webAPIService)
        {
            _webAPIService = webAPIService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> AddFaculty(string facultyName)
        {
            if (string.IsNullOrEmpty(facultyName))
            {
                ModelState.AddModelError("", "Faculty name is required");
                return View();
            }

            try
            {
                var result = await _webAPIService.AddFacultyAsync(facultyName);

                if (result.Success)
                {
                    TempData["Success"] = $"Faculty '{facultyName}' added successfully";
                    return RedirectToAction("Index");
                }

                ModelState.AddModelError("", result.ErrorMessage);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error: " + ex.Message);
            }

            return View();
        }
    }
}