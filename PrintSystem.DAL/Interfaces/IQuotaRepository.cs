using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PrintSystem.Models;

namespace PrintSystem.DAL.Interfaces
{
    public interface IQuotaRepository
    {
        Task<bool> AddAmountAsync(string username, float quotas);
        Task<float> GetAvailableAmountAsync(string username);
        Task<bool> DeductAmountAsync(string username, float amount);
    }
}