using Microsoft.AspNetCore.Mvc;
using MVC_PrintSystem.Models;

namespace MVC_PrintSystem.Services
{
    public interface IFacultyStudentPrintPaymentServices
    {
        Task<IActionResult> Index();
        Task<IActionResult> FilterStudents(FacultyStudentsListViewModel viewModel);
        Task<IActionResult> Payment(double selectedStudentId);
        Task<IActionResult> GetPrintQuota(double amount, int facultyId, int selectedAccountId);
        Task<IActionResult> MakePayments(double amount, int selectedAccountId);
    }
}
