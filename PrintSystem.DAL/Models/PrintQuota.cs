using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintSystem.DAL.Models
{
    public class PrintQuota
    {
        public int PrintQuotaId { get; set; }
        public string Description { get; set; }
        public double Amount { get; set; }
        public int FacultyId { get; set; }
        public Faculty Faculty { get; set; }

    }
}
