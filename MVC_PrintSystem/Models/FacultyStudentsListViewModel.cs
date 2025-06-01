namespace MVC_PrintSystem.Models
{
    public class FacultyStudentsListViewModel
    {
        public int FacultyId { get; set; }

        public string FacultyName { get; set; }

        public List<StudentViewModel> Students { get; set; } = new List<StudentViewModel>();

        // Optional filters
        public string? SearchTerm { get; set; }
    }
}

