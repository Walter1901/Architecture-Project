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

        // Navigation properties
        public User? AccountOwner { get; set; }

        // Navigation property for payments
        public virtual ICollection<Payment> DebitPayments { get; set; } = new List<Payment>();
        public virtual ICollection<Payment> CreditPayments { get; set; } = new List<Payment>();
    }
}
