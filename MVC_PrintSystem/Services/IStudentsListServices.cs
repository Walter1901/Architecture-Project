using Microsoft.AspNetCore.Mvc;
using MVC_PrintSystem.Models;

namespace MVC_PrintSystem.Services
{
    public interface IStudentsListServices
    {
        Task<IActionResult> Index();
        Task<IActionResult> Filter(FacultyStudentsListViewModel filter);
        Task<IActionResult> ViewDetails(int selectedStudentId);
    }
}
