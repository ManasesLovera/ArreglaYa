using Microsoft.EntityFrameworkCore;
using Domain.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace Infraestructure.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<CompanyService>? CompanyServices { get; set; }
        public DbSet<Transaction>? Transactions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //modelBuilder.HasDefaultSchema("Identity");

            // Configure Identity Tables
            //modelBuilder.Entity<IdentityRole>(roles =>
            //{
            //    roles.ToTable(name: "Roles");
            //});

            //modelBuilder.Entity<IdentityUserRole<string>>(roles =>
            //{
            //    roles.ToTable(name: "UserRoles");
            //});
            
            //modelBuilder.Entity<IdentityUserLogin<string>>(roles =>
            //{
            //    roles.ToTable(name: "UserLogins");
            //});

            modelBuilder.Entity<Transaction>()
                .HasOne(t => t.CompanyService)
                .WithMany(cs => cs.Transactions)
                .HasForeignKey(t => t.CompanyServiceId);
        }

    }
}
