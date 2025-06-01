using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using MVC_PrintSystem.Models;

namespace MVC_PrintSystem.Services
{
    public class StudentsListService : IStudentsListServices
    {
        public async Task<IActionResult> Index()
        {
            return new ViewResult { ViewName = "Index" };
        }

        public async Task<IActionResult> Filter(FacultyStudentsListViewModel filter)
        {
            return new ViewResult { ViewName = "FilteredList", ViewData = new ViewDataDictionary(new EmptyModelMetadataProvider(), new ModelStateDictionary()) { Model = filter } };
        }

        public async Task<IActionResult> ViewDetails(int selectedStudentId)
        {
            return new ViewResult { ViewName = "ViewDetails", ViewData = new ViewDataDictionary(new EmptyModelMetadataProvider(), new ModelStateDictionary()) { Model = selectedStudentId } };
        }
    }
}
