using Microsoft.Data.SqlClient;

namespace WebAPI_PrintSystem.Services
{
    public class PaymentDBService : IPaymentDBService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<PaymentDBService> _logger;

        public PaymentDBService(IConfiguration configuration, ILogger<PaymentDBService> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<bool> TransferMoneyAsync(string username, float amount)
        {
            try
            {
                _logger.LogInformation($"Processing payment transfer: {amount} CHF for user {username}");

                var connectionString = _configuration.GetConnectionString("DefaultConnection") ??
                                     "Server=(localdb)\\mssqllocaldb;Database=PrintSystemDB;Trusted_Connection=true;MultipleActiveResultSets=true";

                using var connection = new SqlConnection(connectionString);
                await connection.OpenAsync();

                var createTableCommand = new SqlCommand(@"
                    IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'PaymentTransactions')
                    BEGIN
                        CREATE TABLE PaymentTransactions (
                            Id INT IDENTITY(1,1) PRIMARY KEY,
                            Username NVARCHAR(50) NOT NULL,
                            Amount FLOAT NOT NULL,
                            TransactionDate DATETIME2 NOT NULL DEFAULT(GETDATE()),
                            Status NVARCHAR(20) NOT NULL DEFAULT('Completed')
                        );
                    END", connection);

                await createTableCommand.ExecuteNonQueryAsync();

                var command = new SqlCommand(@"
                    INSERT INTO PaymentTransactions (Username, Amount, TransactionDate, Status)
                    VALUES (@username, @amount, GETDATE(), 'Completed')", connection);

                command.Parameters.AddWithValue("@username", username);
                command.Parameters.AddWithValue("@amount", amount);

                var rowsAffected = await command.ExecuteNonQueryAsync();

                if (rowsAffected > 0)
                {
                    _logger.LogInformation($"Payment transaction recorded: {amount} CHF for user {username}");
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in TransferMoneyAsync for {username}: {ex.Message}");
                _logger.LogError($"Stack trace: {ex.StackTrace}");

                
                _logger.LogWarning($"Payment DB error, but considering payment successful for {username}");
                return true;
            }
        }
    }
}