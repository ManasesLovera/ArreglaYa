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

        public DbSet<Admin>? Admins { get; set; }
        public DbSet<Company>? Companies { get; set; }
        public DbSet<Client>? Clients { get; set; }
        public DbSet<CompanyService>? CompanyServices { get; set; }
        public DbSet<Transaction>? Transactions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.HasDefaultSchema("Identity");

            // Configure Identity Tables
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
            
            // Separate Tables for Each User Type
            modelBuilder.Entity<Admin>().ToTable("Admins");
            modelBuilder.Entity<Company>().ToTable("Companies");
            modelBuilder.Entity<Client>().ToTable("Clients");

            // Relationships
            modelBuilder.Entity<Transaction>()
                .HasOne(t => t.Client)
                .WithMany(c => c.Transactions)
                .HasForeignKey(t => t.ClientId);

            modelBuilder.Entity<CompanyService>()
                .HasOne(cs => cs.Company)
                .WithMany(c => c.CompanyServices)
                .HasForeignKey(cs => cs.CompanyId);

            modelBuilder.Entity<Transaction>()
                .HasOne(t => t.CompanyService)
                .WithMany(cs => cs.Transactions)
                .HasForeignKey(t => t.CompanyServiceId);
        }

    }
}
