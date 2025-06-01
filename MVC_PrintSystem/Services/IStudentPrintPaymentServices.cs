using Microsoft.AspNetCore.Mvc;

namespace MVC_PrintSystem.Services
{
    public interface IStudentPrintPaymentServices
    {
        Task<IActionResult> GetPrintQuota(double amount, int facultyId);
        Task<IActionResult> MakePayment(double amount);
    }
}
