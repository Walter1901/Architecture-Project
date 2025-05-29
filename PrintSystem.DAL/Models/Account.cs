using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintSystem.DAL.Models
{
    public class Account
    {
        public int AccountId { get; set; }
        public double Balance { get; set; }
        public int? AccountOwnerId { get; set; }
        public User? AccountOwner { get; set; }
    }
}
