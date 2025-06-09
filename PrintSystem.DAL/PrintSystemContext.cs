using Microsoft.EntityFrameworkCore;
using PrintSystem.DAL.Models;

namespace PrintSystem.DAL
{
    public class PrintSystemContext : DbContext
    {
        public PrintSystemContext(DbContextOptions<PrintSystemContext> options)
            : base(options)
        {
        }

        public DbSet<Account> Accounts { get; set; }
        public DbSet<Card> Cards { get; set; }
        public DbSet<Class> Classes { get; set; }
        public DbSet<Faculty> Faculties { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Printer> Printers { get; set; }
        public DbSet<PrintQuota> PrintQuotas { get; set; }
        public DbSet<Sector> Sectors { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Payment>()
                .HasOne(p => p.Debit)
                .WithMany()
                .HasForeignKey(p => p.DebitAccountId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Payment>()
                .HasOne(p => p.Credit)
                .WithMany()
                .HasForeignKey(p => p.CreditAccountId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Student>()
                .HasOne(s => s.PrintAccount)
                .WithOne(a => (Student)a.AccountOwner) 
                .HasForeignKey<Student>(s => s.printAccountId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Faculty>()
                .HasOne(f => f.Account)
                .WithMany()
                .HasForeignKey(f => f.AccountId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Card>()
                .HasOne(c => c.CardOwner)
                .WithMany()
                .HasForeignKey(c => c.CardOwnerId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Card>()
                .HasOne(c => c.Faculty)
                .WithMany()
                .HasForeignKey(c => c.FacultyId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Sector>()
                .HasOne(s => s.Faculty)
                .WithMany()
                .HasForeignKey(s => s.FacultyId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Class>()
                .HasOne(c => c.Sector)
                .WithMany()
                .HasForeignKey(c => c.SectorId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<PrintQuota>()
                .HasOne(pq => pq.Faculty)
                .WithMany()
                .HasForeignKey(pq => pq.FacultyId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Printer>()
                .HasOne(p => p.Faculty)
                .WithMany()
                .HasForeignKey(p => p.FacultyId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Student>()
                .HasIndex(s => new { s.FirstName, s.LastName })
                .HasDatabaseName("IX_Student_Name");

            modelBuilder.Entity<Account>()
                .HasIndex(a => a.AccountOwnerId)
                .HasDatabaseName("IX_Account_Owner");

            modelBuilder.Entity<Account>()
                .Property(a => a.Balance)
                .HasPrecision(18, 2); 

            modelBuilder.Entity<Payment>()
                .Property(p => p.Amount)
                .HasPrecision(18, 2);
        }
    }
}