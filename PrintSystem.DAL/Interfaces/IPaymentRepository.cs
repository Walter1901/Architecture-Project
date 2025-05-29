using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PrintSystem.Models;


namespace PrintSystem.DAL.Interfaces
{
    public interface IPaymentRepository
    {
        Task<bool> TransferMoneyAsync(string username, float amount);
        Task<List<PaymentTransaction>> GetPaymentHistoryAsync(string username);
    }
}
