using Microsoft.EntityFrameworkCore;
using Domain.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace Infraestructure.Data
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Admin> Admins { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<CompanyService> CompanyServices { get; set; }
        public DbSet<Transaction> Transactions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.HasDefaultSchema("Identity");

            modelBuilder.Entity<Admin>(entity =>
            {
                entity.ToTable(name: "Admin");
            });

            modelBuilder.Entity<Client>(entity =>
            {
                entity.ToTable(name: "Client");
            });

            modelBuilder.Entity<Company>(entity =>
            {
                entity.ToTable(name: "Company");
            });

            modelBuilder.Entity<IdentityRole>(roles =>
            {
                roles.ToTable(name: "Roles");
            });

            modelBuilder.Entity<IdentityUserRole<string>>(roles =>
            {
                roles.ToTable(name: "UserRoles");
            });

            modelBuilder.Entity<IdentityUserLogin<string>>(roles =>
            {
                roles.ToTable(name: "UserLogins");
            });

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
        }

    }
}
