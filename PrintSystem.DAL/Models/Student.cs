using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintSystem.DAL.Models
{
    public class Student : User
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateOnly DateOfBirth { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public int printAccountId { get; set; }
        public Account PrintAccount { get; set; }

        public override string GetUserRole() => "Student";
    }
}
