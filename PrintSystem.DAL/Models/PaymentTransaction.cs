using System;

namespace PrintSystem.DAL.Models
{
    public class PaymentTransaction
    {
        public string Username { get; set; }
        public float Amount { get; set; }
        public string TransactionType { get; set; }
        public DateTime TransactionDate { get; set; }
        public string Reference { get; set; }
    }
}