using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient; 
using System.Threading.Tasks;
using PrintSystem.DAL.Interfaces;
using PrintSystem.Models;

namespace PrintSystem.DAL.Repositories
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly string _connectionString;

        public PaymentRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<bool> TransferMoneyAsync(string username, float amount)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    // Create table if it doesn't exist
                    using (var createTableCommand = new SqlCommand(@"
                        IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='PaymentTransactions' AND xtype='U')
                        CREATE TABLE PaymentTransactions (
                            Id INT IDENTITY(1,1) PRIMARY KEY,
                            Username NVARCHAR(50) NOT NULL,
                            Amount FLOAT NOT NULL,
                            TransactionDate DATETIME2 NOT NULL DEFAULT(GETDATE()),
                            Status NVARCHAR(20) NOT NULL DEFAULT('Completed')
                        )", connection))
                    {
                        await createTableCommand.ExecuteNonQueryAsync();
                    }

                    using (var command = new SqlCommand(@"
                        INSERT INTO PaymentTransactions (Username, Amount, TransactionDate, Status)
                        VALUES (@username, @amount, GETDATE(), 'Completed')", connection))
                    {
                        command.Parameters.AddWithValue("@username", username);
                        command.Parameters.AddWithValue("@amount", amount);

                        var rowsAffected = await command.ExecuteNonQueryAsync();
                        return rowsAffected > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in TransferMoneyAsync: {ex.Message}");
                return false;
            }
        }

        public async Task<List<PaymentTransaction>> GetPaymentHistoryAsync(string username)
        {
            var transactions = new List<PaymentTransaction>();

            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    using (var command = new SqlCommand(
                        "SELECT * FROM PaymentTransactions WHERE Username = @username ORDER BY TransactionDate DESC",
                        connection))
                    {
                        command.Parameters.AddWithValue("@username", username);

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                transactions.Add(new PaymentTransaction
                                {
                                    Username = reader["Username"].ToString(),
                                    Amount = Convert.ToSingle(reader["Amount"]),
                                    TransactionDate = Convert.ToDateTime(reader["TransactionDate"]),
                                    TransactionType = "Online",
                                    Reference = reader["Id"].ToString()
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetPaymentHistoryAsync: {ex.Message}");
            }

            return transactions;
        }
    }
}