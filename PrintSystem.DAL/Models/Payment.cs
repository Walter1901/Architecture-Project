using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintSystem.DAL.Models
{
    public class Payment
    {
        public int PaymentId { get; set; }
        public double Amount { get; set; }
        public DateOnly Date { get; set; }
        public int DebitAccountId { get; set; }
        public Account Debit { get; set; }
        public int CreditAccountId { get; set; }
        public Account Credit { get; set; }
    }
}
