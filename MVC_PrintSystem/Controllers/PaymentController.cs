using Microsoft.AspNetCore.Mvc;
using MVC_PrintSystem.Services;

namespace MVC_PrintSystem.Controllers
{
    public class PaymentController : Controller
    {
        private readonly IWebAPIService _webAPIService;

        public PaymentController(IWebAPIService webAPIService)
        {
            _webAPIService = webAPIService;
        }

        public IActionResult ProcessPayment() => View();

        [HttpPost]
        public async Task<IActionResult> ProcessPayment(string username, float amount)
        {
            if (string.IsNullOrEmpty(username) || amount <= 0)
            {
                ModelState.AddModelError("", "Invalid username or amount");
                return View();
            }

            var result = await _webAPIService.ProcessOnlinePaymentAsync(username, amount);
            if (result.Success)
            {
                TempData["Success"] = $"Payment of {amount} processed successfully for {username}";
                return RedirectToAction("ProcessPayment");
            }

            ModelState.AddModelError("", result.ErrorMessage);
            return View();
        }
    }
}