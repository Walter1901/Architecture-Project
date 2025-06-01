using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace MVC_PrintSystem.Services
{
    public class FacultyHomeService : IFacultyHomeServices
    {
        public async Task<IActionResult> ViewStudents()
        {
            // Return a sample list of students
            var students = new List<string> { "Anna", "Ben", "Carla" };
            return new ViewResult { ViewName = "ViewStudents", ViewData = new ViewDataDictionary(new EmptyModelMetadataProvider(), new ModelStateDictionary()) { Model = students } };
        }

        public async Task<IActionResult> ManagePrintPayments()
        {
            return new ViewResult { ViewName = "ManagePrintPayments" };
        }
    }
}
