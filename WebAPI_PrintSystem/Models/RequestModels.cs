namespace WebAPI_PrintSystem.Models
{
    public class AddAmountRequest
    {
        public string Username { get; set; } = string.Empty;
        public float Quotas { get; set; }
    }

    public class OnlinePaymentRequest
    {
        public string Username { get; set; } = string.Empty;
        public float Amount { get; set; }
    }

    public class PrintCheckRequest
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public float Amount { get; set; }
    }

    public class PrintDeductRequest
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public float Amount { get; set; }
    }

    public class SAPUserData
    {
        public string Username { get; set; } = string.Empty;
        public string UID { get; set; } = string.Empty;
    }
}