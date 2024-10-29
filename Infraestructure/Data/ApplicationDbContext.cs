using Microsoft.EntityFrameworkCore;
using Domain.Models;

namespace Infraestructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Admin> Admins { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<CompanyService> CompanyServices { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //client y transaction
            modelBuilder.Entity<Transaction>()
                .HasOne(t => t.Client)
                .WithMany(c => c.Transactions)
                .HasForeignKey(t => t.ClientId);

            //company y companyService
            modelBuilder.Entity<CompanyService>()
                .HasOne(cs => cs.Company)
                .WithMany(c => c.CompanyServices)
                .HasForeignKey(cs => cs.CompanyId);

            // companyService y transaction
            modelBuilder.Entity<Transaction>()
                .HasOne(t => t.CompanyService)
                .WithMany(cs => cs.Transactions)
                .HasForeignKey(t => t.CompanyServiceId);

            // companyService y user
            modelBuilder.Entity<CompanyService>()
                .HasOne(cs => cs.User)
                .WithMany()
                .HasForeignKey(cs => cs.UserId);
        }

    }
}
