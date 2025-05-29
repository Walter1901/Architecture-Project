using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintSystem.DAL.Models
{
    public class Card
    {
        public int CardId { get; set; }
        public DateOnly ExpirationDate { get; set; }
        public int? CardOwnerId { get; set; }
        public User? CardOwner { get; set; }
        public int FacultyId { get; set; }
        public Faculty Faculty { get; set; }
    }
}
