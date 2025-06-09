using Microsoft.AspNetCore.Mvc;
using MVC_PrintSystem.Models.ViewModels;
using MVC_PrintSystem.Services;
using PrintSystem.Models;

namespace MVC_PrintSystem.Controllers
{
    // MVC Controller handling student-related views and operations
    // Manages student dashboard, payment interface, and account top-up
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

        // Displays the online payment form for account top-up
        // Shows current balance and top-up options in CHF
        [HttpGet]
        public IActionResult PayOnline()
        {
            // Check if user is authenticated as student
            if (!CheckRole("Student"))
                return RedirectToLogin("Access denied - Student access required");

            // Set view data for form constraints and user info
            ViewBag.Username = CurrentUsername;
            ViewBag.MinAmount = 5;   // Minimum top-up amount in CHF
            ViewBag.MaxAmount = 100; // Maximum top-up amount in CHF
            ViewBag.Currency = "CHF";
            return View();
        }

        // Processes the online payment form submission
        // Validates input and calls API to process payment in CHF
        [HttpPost]
        public async Task<IActionResult> PayOnline(TopUpViewModel model)
        {
            if (!CheckRole("Student"))
                return RedirectToLogin("Access denied - Student access required");

            // Check model validation (amount range, required fields)
            if (!ModelState.IsValid)
            {
                ViewBag.Username = CurrentUsername;
                ViewBag.MinAmount = 5;
                ViewBag.MaxAmount = 100;
                ViewBag.Currency = "CHF";
                return View(model);
            }

            try
            {
                // Create payment request and call API
                var result = await _webAPIService.ProcessOnlinePaymentAsync(CurrentUsername, model.Amount);

                if (result.Success)
                {
                    // Success - set confirmation message and redirect
                    TempData["Success"] = $"Payment of {model.Amount} CHF processed successfully!";
                    TempData["TransactionId"] = "TXN_" + DateTime.Now.ToString("yyyyMMddHHmmss");
                    return RedirectToAction("Dashboard");
                }

                // API returned error
                ModelState.AddModelError("", result.ErrorMessage);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Payment error: " + ex.Message);
            }

            // Return to form with errors
            ViewBag.Username = CurrentUsername;
            ViewBag.MinAmount = 5;
            ViewBag.MaxAmount = 100;
            ViewBag.Currency = "CHF";
            return View(model);
        }

        // Displays payment transaction history for the current student
        // Shows all top-ups, print deductions, and account activity in CHF
        [HttpGet]
        public async Task<IActionResult> PaymentHistory()
        {
            if (!CheckRole("Student"))
                return RedirectToLogin("Access denied - Student access required");

            try
            {
                // In production, call API to get real transaction history
                // For now, using mock data
                var history = new List<PaymentTransaction>
                {
                    new PaymentTransaction
                    {
                        Username = CurrentUsername,
                        Amount = 20.0f, // Top-up in CHF
                        TransactionType = "TopUp",
                        TransactionDate = DateTime.Now.AddDays(-5),
                        Reference = "TXN_20241201_ABC123"
                    },
                    new PaymentTransaction
                    {
                        Username = CurrentUsername,
                        Amount = -1.60f, // Print deduction in CHF
                        TransactionType = "Print",
                        TransactionDate = DateTime.Now.AddDays(-2),
                        Reference = "PRINT_20241204_DEF456"
                    }
                };

                ViewBag.Username = CurrentUsername;
                ViewBag.Currency = "CHF";
                return View(history);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View(new List<PaymentTransaction>());
            }
        }
    }
}
