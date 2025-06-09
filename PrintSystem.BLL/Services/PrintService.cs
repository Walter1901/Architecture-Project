using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PrintSystem.BLL.Interfaces;

namespace PrintSystem.BLL.Services
{
    public class PrintService : IPrintService
    {
        private static List<string> _printJobs = new List<string>();
        private static int _jobCounter = 0;

        public async Task<string> CreatePrintJobAsync(string documentName, int copies)
        {
            await Task.Delay(100); // Simulation d'une opération async

            _jobCounter++;
            string jobInfo = $"Job #{_jobCounter}: {documentName} - {copies} copies";
            _printJobs.Add(jobInfo);

            return $"Print job created: {jobInfo}";
        }

        public async Task<List<string>> GetAllPrintJobsAsync()
        {
            await Task.Delay(50);
            return new List<string>(_printJobs);
        }

        public async Task<bool> CancelPrintJobAsync(int jobId)
        {
            await Task.Delay(50);

            if (jobId > 0 && jobId <= _printJobs.Count)
            {
                _printJobs.RemoveAt(jobId - 1);
                return true;
            }
            return false;
        }

        public async Task<string> GetPrintStatusAsync(int jobId)
        {
            await Task.Delay(50);

            if (jobId > 0 && jobId <= _printJobs.Count)
            {
                return $"Status for Job #{jobId}: Completed";
            }
            return "Job not found";
        }
    }
}