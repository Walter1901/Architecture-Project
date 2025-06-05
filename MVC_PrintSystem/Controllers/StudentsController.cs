using Microsoft.AspNetCore.Mvc;
using MVC_PrintSystem.Models.ViewModels;
using MVC_PrintSystem.Services;

namespace MVC_PrintSystem.Controllers
{
    public class StudentsController : BaseController
    {
        private readonly IWebAPIService _webAPIService;

        public StudentsController(IWebAPIService webAPIService)
        {
            _webAPIService = webAPIService;
        }

        public async Task<IActionResult> Dashboard()
        {
            
            if (!CheckRole("Student"))
                return RedirectToLogin("Access denied - Student access required");

            try
            {
                var availableAmount = await _webAPIService.GetAvailableAmountAsync(CurrentUsername);
                ViewBag.Username = CurrentUsername;
                ViewBag.AvailableAmount = availableAmount;

                return View();
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View();
            }
        }

        public IActionResult PayOnline()
        {
            
            if (!CheckRole("Student"))
                return RedirectToLogin("Access denied - Student access required");

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> PayOnline(TopUpViewModel model)
        {
            if (!CheckRole("Student"))
                return RedirectToLogin("Access denied - Student access required");

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                var result = await _webAPIService.ProcessOnlinePaymentAsync(CurrentUsername, model.Amount);

                if (result.Success)
                {
                    TempData["Success"] = $"Payment of {model.Amount} CHF processed successfully";
                    return RedirectToAction("Dashboard");
                }

                ModelState.AddModelError("", result.ErrorMessage);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Payment error: " + ex.Message);
            }

            return View(model);
        }
    }
}