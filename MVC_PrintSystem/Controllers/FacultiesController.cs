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
                // Utilisez PrintSystem.Models.User explicitement
                var students = await _webAPIService.GetFacultyStudentsAsync(CurrentUserFaculty ?? "DefaultFaculty");
                ViewBag.Username = CurrentUsername;
                ViewBag.Faculty = CurrentUserFaculty;
                ViewBag.TotalStudents = students?.Count ?? 0;
                ViewBag.TotalQuotaAllocated = students?.Sum(s => s.AvailableQuota) ?? 0;

                // Retournez une liste vide si students est null
                return View(students ?? new List<PrintSystem.Models.User>());
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View(new List<PrintSystem.Models.User>());
            }
        }

        public IActionResult AllocateQuota()
        {
            if (!CheckRole("Faculty"))
                return RedirectToLogin("Access denied - Faculty access required");

            ViewBag.MaxAllocation = 50;
            ViewBag.Faculty = CurrentUserFaculty ?? "Computer Science";
            ViewBag.Currency = "CHF";
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AllocateQuota(string username, float amount, string reason)
        {
            if (!CheckRole("Faculty"))
                return RedirectToLogin("Access denied - Faculty access required");

            if (string.IsNullOrEmpty(username) || amount <= 0)
            {
                ModelState.AddModelError("", "Valid username and amount are required");
                ViewBag.MaxAllocation = 50;
                ViewBag.Faculty = CurrentUserFaculty ?? "Computer Science";
                ViewBag.Currency = "CHF";
                return View();
            }

            if (amount > 50)
            {
                ModelState.AddModelError("", "Maximum allocation: 50 CHF per student");
                ViewBag.MaxAllocation = 50;
                ViewBag.Faculty = CurrentUserFaculty ?? "Computer Science";
                ViewBag.Currency = "CHF";
                return View();
            }

            try
            {
                var result = await _webAPIService.AddAmountAsync(username, amount);

                if (result.Success)
                {
                    TempData["Success"] = $"Quota of {amount} CHF allocated to {username} successfully!";
                    TempData["AllocationDetails"] = $"Allocated by: {CurrentUsername}, Reason: {reason ?? "Not specified"}";
                    return RedirectToAction("Dashboard");
                }

                ModelState.AddModelError("", result.ErrorMessage);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Allocation error: " + ex.Message);
            }

            ViewBag.MaxAllocation = 50;
            ViewBag.Faculty = CurrentUserFaculty ?? "Computer Science";
            ViewBag.Currency = "CHF";
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> QuickAllocate(string username, float amount)
        {
            if (!CheckRole("Faculty"))
                return Json(new { success = false, message = "Access denied" });

            try
            {
                var result = await _webAPIService.AddAmountAsync(username, amount);

                if (result.Success)
                {
                    var newBalance = await _webAPIService.GetAvailableAmountAsync(username);
                    return Json(new
                    {
                        success = true,
                        message = $"Quota of {amount} CHF added successfully",
                        newBalance = newBalance,
                        currency = "CHF"
                    });
                }

                return Json(new { success = false, message = result.ErrorMessage });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> StudentDetails(string username)
        {
            if (!CheckRole("Faculty"))
                return RedirectToLogin("Access denied - Faculty access required");

            try
            {
                var quota = await _webAPIService.GetAvailableAmountAsync(username);

                var studentDetails = new
                {
                    Username = username,
                    AvailableQuota = quota,
                    Currency = "CHF",
                    Faculty = CurrentUserFaculty ?? "Computer Science",
                    LastActivity = DateTime.Now.AddDays(-2),
                    TotalAllocations = 3,
                    TotalPrintJobs = 15
                };

                ViewBag.StudentDetails = studentDetails;
                return View();
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View();
            }
        }
    }

}