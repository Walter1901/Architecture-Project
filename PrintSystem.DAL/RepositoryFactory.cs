using PrintSystem.DAL.Interfaces;
using PrintSystem.DAL.Repositories;
using PrintSystem.Models;

namespace PrintSystem.DAL
{
    public class RepositoryFactory
    {
        private readonly string _connectionString;

        public RepositoryFactory(string connectionString)
        {
            _connectionString = connectionString;
        }

        public IQuotaRepository CreateQuotaRepository()
        {
            return new QuotaRepository(_connectionString);
        }

        public IPaymentRepository CreatePaymentRepository()
        {
            return new PaymentRepository(_connectionString);
        }
    }
}