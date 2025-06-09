using Microsoft.AspNetCore.Mvc;

namespace MVC_PrintSystem.Services
{
    public interface IFacultyHomeServices
    {
        Task<IActionResult> ViewStudents();
        Task<IActionResult> ManagePrintPayments();
    }
}
