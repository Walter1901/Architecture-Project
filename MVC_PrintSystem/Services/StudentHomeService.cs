using Microsoft.AspNetCore.Mvc;

namespace MVC_PrintSystem.Services
{
    public class StudentHomeService : IStudentHomeServices
    {
        public async Task<IActionResult> PaymentPrintPage()
        {
            // Simulated page logic or data fetching
            var printQuota = new
            {
                RemainingPages = 42,
                LastRecharge = DateTime.Now.AddDays(-7)
            };

            return new ViewResult
            {
                ViewName = "PaymentPrintPage",
                ViewData = new Microsoft.AspNetCore.Mvc.ViewFeatures.ViewDataDictionary(
                    new Microsoft.AspNetCore.Mvc.ModelBinding.EmptyModelMetadataProvider(),
                    new Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateDictionary())
                {
                    Model = printQuota
                }
            };
        }
    }
}
