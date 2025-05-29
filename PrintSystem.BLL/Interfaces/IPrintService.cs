using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PrintSystem.BLL.Interfaces
{
    public interface IPrintService
    {
        Task<string> CreatePrintJobAsync(string documentName, int copies);
        Task<List<string>> GetAllPrintJobsAsync();
        Task<bool> CancelPrintJobAsync(int jobId);
        Task<string> GetPrintStatusAsync(int jobId);
    }
}