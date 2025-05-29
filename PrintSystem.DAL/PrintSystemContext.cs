using Microsoft.EntityFrameworkCore;
using PrintSystem.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            
        }

    }
}
