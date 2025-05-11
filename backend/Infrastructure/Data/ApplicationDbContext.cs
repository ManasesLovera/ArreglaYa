using Microsoft.EntityFrameworkCore;
using Domain.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        public DbSet<CompanyService>? CompanyServices { get; set; }
        public DbSet<Transaction>? Transactions { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            #region Tables
            builder.Entity<ApplicationUser>()
                .ToTable("Users");
            #endregion
            #region SQL Relationships
            // CompanyService -> ApplicationUser (CompanyId as FK)
            builder.Entity<CompanyService>()
                .HasOne(cs => cs.Company)
                .WithMany(u => u.CompanyServices)
                .HasForeignKey(cs => cs.CompanyId)
                .OnDelete(DeleteBehavior.Restrict);

            // Transaction -> CompanyService
            builder.Entity<Transaction>()
                .HasOne(t => t.CompanyService)
                .WithMany(cs => cs.Transactions)
                .HasForeignKey(t => t.CompanyServiceId)
                .OnDelete(DeleteBehavior.Restrict);

            // Transaction -> ApplicationUser (ClientId as FK)
            builder.Entity<Transaction>()
                .HasOne(t => t.Client)
                .WithMany(u => u.Transactions)
                .HasForeignKey(t => t.ClientId)
                .OnDelete(DeleteBehavior.Restrict);
            #endregion
            #region Properties
            builder.Entity<Transaction>()
                .Property(t => t.Status)
                .HasConversion<string>();
            #endregion
        }
    }
}
