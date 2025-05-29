using Microsoft.EntityFrameworkCore;
using PrintSystem.Models;

namespace WebAPI_PrintSystem.Data
{
    public class PrintSystemDB : DbContext
    {
        public PrintSystemDB(DbContextOptions<PrintSystemDB> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<QuotaAllocation> QuotaAllocations { get; set; }
        public DbSet<PaymentTransaction> PaymentTransactions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configurations
            modelBuilder.Entity<User>().HasKey(u => u.Username);
        }
    }
}