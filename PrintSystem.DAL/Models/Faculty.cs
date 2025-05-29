using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintSystem.DAL.Models
{
    public class Faculty : User
    {
        public string FacultyName { get; set; }
        public int AccountId { get; set; }
        public Account Account { get; set; }

    }
}
