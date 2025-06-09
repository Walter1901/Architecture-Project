using Microsoft.AspNetCore.Mvc;
using MVC_PrintSystem.Services;

namespace MVC_PrintSystem.Controllers
{
    public class QuotaController : Controller
    {
        private readonly IWebAPIService _webAPIService;

        public QuotaController(IWebAPIService webAPIService)
        {
            _webAPIService = webAPIService;
        }

        public IActionResult AddQuota() => View();

        [HttpPost]
        public async Task<IActionResult> AddQuota(string username, float quotas)
        {
            if (string.IsNullOrEmpty(username) || quotas <= 0)
            {
                ModelState.AddModelError("", "Invalid username or quota amount");
                return View();
            }

            var result = await _webAPIService.AddAmountAsync(username, quotas);
            if (result.Success)
            {
                TempData["Success"] = $"Quota of {quotas} added for {username}";
                return RedirectToAction("AddQuota");
            }

            ModelState.AddModelError("", result.ErrorMessage);
            return View();
        }

        public async Task<IActionResult> AvailableQuota(string username)
        {
            var quota = await _webAPIService.GetAvailableAmountAsync(username);
            ViewBag.Quota = quota;
            return View();
        }
    }
}