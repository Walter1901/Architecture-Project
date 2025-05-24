using Microsoft.Data.SqlClient;

namespace WebAPI_PrintSystem.Services
{
    public class PaymentDBService : IPaymentDBService
    {
        private readonly IConfiguration _configuration;

        public PaymentDBService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<bool> TransferMoneyAsync(string username, float amount)
        {
            try
            {
                var connectionString = _configuration.GetConnectionString("DefaultConnection") ??
                                     "Server=(localdb)\\mssqllocaldb;Database=PrintSystemDB;Trusted_Connection=true;MultipleActiveResultSets=true";

                using var connection = new SqlConnection(connectionString);
                await connection.OpenAsync();

                // Create table if it doesn't exist
                var createTableCommand = new SqlCommand(@"
                    IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='PaymentTransactions' AND xtype='U')
                    CREATE TABLE PaymentTransactions (
                        Id INT IDENTITY(1,1) PRIMARY KEY,
                        Username NVARCHAR(50) NOT NULL,
                        Amount FLOAT NOT NULL,
                        TransactionDate DATETIME2 NOT NULL DEFAULT(GETDATE()),
                        Status NVARCHAR(20) NOT NULL DEFAULT('Completed')
                    )", connection);

                await createTableCommand.ExecuteNonQueryAsync();

                var command = new SqlCommand(@"
                    INSERT INTO PaymentTransactions (Username, Amount, TransactionDate, Status)
                    VALUES (@username, @amount, GETDATE(), 'Completed')", connection);

                command.Parameters.AddWithValue("@username", username);
                command.Parameters.AddWithValue("@amount", amount);

                var rowsAffected = await command.ExecuteNonQueryAsync();
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in TransferMoneyAsync: {ex.Message}");
                return false;
            }
        }
    }
}