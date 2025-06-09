using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace MVC_PrintSystem.Services
{
    public class StudentPrintPaymentService : IStudentPrintPaymentServices
    {
        private readonly HttpClient _http;

        public StudentPrintPaymentService(HttpClient http)
        {
            _http = http;
        }

        public async Task<IActionResult> GetPrintQuota(double amount, int facultyId)
        {
            // Simulated API response
            var quota = new { Amount = 10.0, FacultyId = facultyId };
            return new ViewResult { ViewName = "Quota", ViewData = new Microsoft.AspNetCore.Mvc.ViewFeatures.ViewDataDictionary(new EmptyModelMetadataProvider(), new ModelStateDictionary()) { Model = quota } };
        }

        public async Task<IActionResult> MakePayment(double amount)
        {
            // Simulated logic or POST to /api/payment/online
            return new RedirectToActionResult("Index", "StudentPrintPayment", null);
        }
    }
}
