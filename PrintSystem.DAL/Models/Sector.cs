using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintSystem.DAL.Models
{
    public class Sector
    {
        public int SectorId { get; set; }
        public string SectorName { get; set; }
        public int FacultyId { get; set; }
        public Faculty Faculty { get; set; }

    }
}
