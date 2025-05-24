using System.Data;
using Microsoft.Data.SqlClient;

namespace WebAPI_PrintSystem.Services
{
    public class SqlService : ISqlService
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;

        public SqlService(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("DefaultConnection") ??
                              "Server=(localdb)\\mssqllocaldb;Database=PrintSystemDB;Trusted_Connection=true;MultipleActiveResultSets=true";
        }

        public async Task<bool> AddAmountAsync(string username, float quotas)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                await connection.OpenAsync();

                // Create table if it doesn't exist
                var createTableCommand = new SqlCommand(@"
                    IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='PrintQuotas' AND xtype='U')
                    CREATE TABLE PrintQuotas (
                        Id INT IDENTITY(1,1) PRIMARY KEY,
                        Username NVARCHAR(50) NOT NULL UNIQUE,
                        AvailableAmount FLOAT NOT NULL DEFAULT(0),
                        LastUpdated DATETIME2 NOT NULL DEFAULT(GETDATE())
                    )", connection);

                await createTableCommand.ExecuteNonQueryAsync();

                // Insert or update quota
                var command = new SqlCommand(@"
                    IF EXISTS (SELECT 1 FROM PrintQuotas WHERE Username = @username)
                    BEGIN
                        UPDATE PrintQuotas 
                        SET AvailableAmount = AvailableAmount + @quotas, 
                            LastUpdated = GETDATE()
                        WHERE Username = @username;
                    END
                    ELSE
                    BEGIN
                        INSERT INTO PrintQuotas (Username, AvailableAmount, LastUpdated)
                        VALUES (@username, @quotas, GETDATE());
                    END", connection);

                command.Parameters.AddWithValue("@username", username);
                command.Parameters.AddWithValue("@quotas", quotas);

                await command.ExecuteNonQueryAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in AddAmountAsync: {ex.Message}");
                return false;
            }
        }

        public async Task<float> GetAvailableAmountAsync(string username)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                await connection.OpenAsync();

                var command = new SqlCommand(
                    "SELECT ISNULL(AvailableAmount, 0) FROM PrintQuotas WHERE Username = @username",
                    connection);
                command.Parameters.AddWithValue("@username", username);

                var result = await command.ExecuteScalarAsync();
                return result != null ? Convert.ToSingle(result) : 0f;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetAvailableAmountAsync: {ex.Message}");
                return 0f;
            }
        }

        public async Task<bool> DeductAmountAsync(string username, float amount)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                await connection.OpenAsync();

                var command = new SqlCommand(@"
                    UPDATE PrintQuotas 
                    SET AvailableAmount = AvailableAmount - @amount,
                        LastUpdated = GETDATE()
                    WHERE Username = @username AND AvailableAmount >= @amount", connection);

                command.Parameters.AddWithValue("@username", username);
                command.Parameters.AddWithValue("@amount", amount);

                var rowsAffected = await command.ExecuteNonQueryAsync();
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in DeductAmountAsync: {ex.Message}");
                return false;
            }
        }
    }
}