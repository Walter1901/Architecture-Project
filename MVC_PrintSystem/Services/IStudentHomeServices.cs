using Microsoft.AspNetCore.Mvc;

namespace MVC_PrintSystem.Services
{
    public interface IStudentHomeServices
    {
        Task<IActionResult> PaymentPrintPage();
    }
}
