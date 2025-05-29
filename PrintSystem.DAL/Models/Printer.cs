using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintSystem.DAL.Models
{
    public class Printer
    {
        public int PrinterId { get; set; }
        public string PrinterName { get; set; }
        public string PrinterLocation { get; set; }
        public int FacultyId { get; set; }
        public Faculty Faculty { get; set; }   

    }
}
