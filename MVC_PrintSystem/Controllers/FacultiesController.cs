using Microsoft.AspNetCore.Mvc;
using MVC_PrintSystem.Services;
using PrintSystem.Models;

namespace MVC_PrintSystem.Controllers
{
    public class FacultiesController : BaseController
    {
        private readonly IWebAPIService _webAPIService;

        public FacultiesController(IWebAPIService webAPIService)
        {
            _webAPIService = webAPIService;
        }

        public async Task<IActionResult> Dashboard()
        {
            
            if (!CheckRole("Faculty"))
                return RedirectToLogin("Access denied - Faculty access required");

            try
            {
                var students = await _webAPIService.GetFacultyStudentsAsync(CurrentUserFaculty ?? "DefaultFaculty");
                ViewBag.Username = CurrentUsername;
                ViewBag.Faculty = CurrentUserFaculty;

                return View(students);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View(new List<User>());
            }
        }

        public IActionResult AllocateQuota()
        {
            if (!CheckRole("Faculty"))
                return RedirectToLogin("Access denied - Faculty access required");

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AllocateQuota(string username, float quotas)
        {
            if (!CheckRole("Faculty"))
                return RedirectToLogin("Access denied - Faculty access required");

            if (string.IsNullOrEmpty(username) || quotas <= 0)
            {
                ModelState.AddModelError("", "Please provide valid username and quota amount");
                return View();
            }

            try
            {
                var result = await _webAPIService.AddAmountAsync(username, quotas);

                if (result.Success)
                {
                    TempData["Success"] = $"Quota of {quotas} CHF added for {username}";
                    return RedirectToAction("Dashboard");
                }

                ModelState.AddModelError("", result.ErrorMessage);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Allocation error: " + ex.Message);
            }

            return View();
        }


    }

}