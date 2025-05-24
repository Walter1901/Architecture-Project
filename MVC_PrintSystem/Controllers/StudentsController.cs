using Microsoft.AspNetCore.Mvc;
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
                var username = "DefaultUser"; // You can get this from user claims later
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
        public async Task<IActionResult> PayOnline(float amount)
        {
            if (amount <= 0)
            {
                ModelState.AddModelError("", "Please provide a valid amount");
                return View();
            }

            try
            {
                var username = "DefaultUser"; // You can get this from user claims later
                var result = await _webAPIService.ProcessOnlinePaymentAsync(username, amount);

                if (result.Success)
                {
                    TempData["Success"] = $"Payment of {amount} CHF processed successfully";
                    return RedirectToAction("Dashboard");
                }

                ModelState.AddModelError("", result.ErrorMessage);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Payment error: " + ex.Message);
            }

            return View();
        }
    }
}