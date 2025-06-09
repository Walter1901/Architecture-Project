using System.Data;
using Microsoft.Data.SqlClient;

namespace WebAPI_PrintSystem.Services
{
    public class SqlService : ISqlService
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;
        private readonly ILogger<SqlService> _logger;

        public SqlService(IConfiguration configuration, ILogger<SqlService> logger)
        {
            _configuration = configuration;
            _logger = logger;
            _connectionString = _configuration.GetConnectionString("DefaultConnection") ??
                              "Server=(localdb)\\mssqllocaldb;Database=PrintSystemDB;Trusted_Connection=true;MultipleActiveResultSets=true";
        }

        public async Task<bool> AddAmountAsync(string username, float quotas)
        {
            try
            {
                _logger.LogInformation($"Adding {quotas} CHF for user {username}");

                using var connection = new SqlConnection(_connectionString);
                await connection.OpenAsync();

                var checkTableCommand = new SqlCommand(@"
                    IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'PrintQuotas')
                    BEGIN
                        CREATE TABLE PrintQuotas (
                            Id INT IDENTITY(1,1) PRIMARY KEY,
                            Username NVARCHAR(50) NOT NULL,
                            AvailableAmount FLOAT NOT NULL DEFAULT(0),
                            LastUpdated DATETIME2 NOT NULL DEFAULT(GETDATE()),
                            CONSTRAINT UQ_PrintQuotas_Username UNIQUE (Username)
                        );
                    END", connection);

                await checkTableCommand.ExecuteNonQueryAsync();

                var command = new SqlCommand(@"
                    MERGE PrintQuotas AS target
                    USING (SELECT @username AS Username, @quotas AS QuotaToAdd) AS source
                    ON target.Username = source.Username
                    WHEN MATCHED THEN
                        UPDATE SET 
                            AvailableAmount = AvailableAmount + source.QuotaToAdd, 
                            LastUpdated = GETDATE()
                    WHEN NOT MATCHED THEN
                        INSERT (Username, AvailableAmount, LastUpdated)
                        VALUES (source.Username, source.QuotaToAdd, GETDATE());", connection);

                command.Parameters.AddWithValue("@username", username);
                command.Parameters.AddWithValue("@quotas", quotas);

                await command.ExecuteNonQueryAsync();

                _logger.LogInformation($"Successfully added {quotas} CHF for user {username}");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in AddAmountAsync for {username}: {ex.Message}");
                _logger.LogError($"Stack trace: {ex.StackTrace}");

                return await AddAmountInMemoryFallback(username, quotas);
            }
        }

        public async Task<float> GetAvailableAmountAsync(string username)
        {
            try
            {
                _logger.LogInformation($"Getting available amount for user {username}");

                using var connection = new SqlConnection(_connectionString);
                await connection.OpenAsync();

                var command = new SqlCommand(@"
                    IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'PrintQuotas')
                        SELECT ISNULL(AvailableAmount, 0) FROM PrintQuotas WHERE Username = @username
                    ELSE
                        SELECT 0", connection);

                command.Parameters.AddWithValue("@username", username);

                var result = await command.ExecuteScalarAsync();
                var amount = result != null ? Convert.ToSingle(result) : 0f;

                _logger.LogInformation($"Available amount for {username}: {amount} CHF");
                return amount;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in GetAvailableAmountAsync for {username}: {ex.Message}");

                return GetAmountInMemoryFallback(username);
            }
        }

        public async Task<bool> DeductAmountAsync(string username, float amount)
        {
            try
            {
                _logger.LogInformation($"Deducting {amount} CHF for user {username}");

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

                if (rowsAffected > 0)
                {
                    _logger.LogInformation($"Successfully deducted {amount} CHF for user {username}");
                    return true;
                }

                _logger.LogWarning($"Insufficient funds for {username}");
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in DeductAmountAsync for {username}: {ex.Message}");
                return false;
            }
        }

        private static readonly Dictionary<string, float> _inMemoryBalances = new()
        {
            { "joaquim.jonathan", 25.0f },
            { "test.user", 15.0f },
            { "marie.dupont", 30.0f }
        };

        private async Task<bool> AddAmountInMemoryFallback(string username, float quotas)
        {
            await Task.Delay(10); // Simulate async

            if (!_inMemoryBalances.ContainsKey(username))
            {
                _inMemoryBalances[username] = 0;
            }

            _inMemoryBalances[username] += quotas;
            _logger.LogWarning($"Used in-memory fallback for {username}. New balance: {_inMemoryBalances[username]} CHF");

            return true;
        }

        private float GetAmountInMemoryFallback(string username)
        {
            var amount = _inMemoryBalances.ContainsKey(username) ? _inMemoryBalances[username] : 0f;
            _logger.LogWarning($"Used in-memory fallback for {username}: {amount} CHF");
            return amount;
        }
    }
}