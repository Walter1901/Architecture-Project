using System.ComponentModel.DataAnnotations;

namespace PrintSystem.Models
{
    public class User
    {
        public string Username { get; set; } = string.Empty;
        public string UID { get; set; } = string.Empty;
        public float AvailableQuota { get; set; }
        public string Faculty { get; set; } = string.Empty;
        public DateTime LastUpdated { get; set; }
        public string Role { get; set; } = string.Empty;
    }

    public class QuotaAllocation
    {
        public string Username { get; set; } = string.Empty;
        public float Amount { get; set; }
        public string AllocatedBy { get; set; } = string.Empty;
        public DateTime AllocationDate { get; set; }
    }

    public class PaymentTransaction
    {
        public string Username { get; set; } = string.Empty;
        public float Amount { get; set; }
        public string TransactionType { get; set; } = string.Empty;
        public DateTime TransactionDate { get; set; }
        public string Reference { get; set; } = string.Empty;
    }

    public class PrintRequest
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public float RequiredAmount { get; set; }
        public string DocumentName { get; set; } = string.Empty;
        public int PageCount { get; set; }
    }

    public class ApiResponse
    {
        public bool Success { get; set; }
        public string ErrorMessage { get; set; } = string.Empty;
        public object? Data { get; set; }
    }
}