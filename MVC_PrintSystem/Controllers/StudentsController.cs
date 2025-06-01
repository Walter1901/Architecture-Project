using Microsoft.AspNetCore.Mvc;
using MVC_PrintSystem.Models.ViewModels;
using MVC_PrintSystem.Services;

namespace MVC_PrintSystem.Controllers
{
    public class StudentsController : Controller
    {
        private readonly IWebAPIService _webAPIService;

        public StudentsController(IWebAPIService webAPIService)
        {
            _webAPIService = webAPIService;
        }

        public async Task<IActionResult> Dashboard()
        {
            try
            {
                var username = User.Identity?.Name;
                var availableAmount = await _webAPIService.GetAvailableAmountAsync(username);
                ViewBag.Username = username;
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
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> PayOnline(TopUpViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                var username = User.Identity?.Name;
                var result = await _webAPIService.ProcessOnlinePaymentAsync(username, model.Amount);

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