using Microsoft.EntityFrameworkCore;
using Domain.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Infrastructure.Data
{
    /// <summary>
    /// Application database context for Entity Framework Core.
    /// Manages database access and entity configurations.
    /// </summary>
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationDbContext"/> class.
        /// </summary>
        /// <param name="options">The database context options.</param>
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        /// <summary>
        /// Gets or sets the DbSet for company services.
        /// </summary>
        public DbSet<CompanyService>? CompanyServices { get; set; }

        /// <summary>
        /// Gets or sets the DbSet for transactions.
        /// </summary>
        public DbSet<Transaction>? Transactions { get; set; }

        /// <summary>
        /// Configures the entity models and relationships.
        /// </summary>
        /// <param name="builder">The model builder for configuring entities.</param>
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            #region Tables
            // Configure ApplicationUser table name
            builder.Entity<ApplicationUser>()
                .ToTable("Users");
            #endregion

            #region SQL Relationships
            // CompanyService -> ApplicationUser (CompanyId as FK)
            // Restrict deletion to prevent orphaned services
            builder.Entity<CompanyService>()
                .HasOne(cs => cs.Company)
                .WithMany(u => u.CompanyServices)
                .HasForeignKey(cs => cs.CompanyId)
                .OnDelete(DeleteBehavior.Restrict);

            // Transaction -> CompanyService
            // Restrict deletion to maintain transaction integrity
            builder.Entity<Transaction>()
                .HasOne(t => t.CompanyService)
                .WithMany(cs => cs.Transactions)
                .HasForeignKey(t => t.CompanyServiceId)
                .OnDelete(DeleteBehavior.Restrict);

            // Transaction -> ApplicationUser (ClientId as FK)
            // Restrict deletion to maintain transaction history
            builder.Entity<Transaction>()
                .HasOne(t => t.Client)
                .WithMany(u => u.Transactions)
                .HasForeignKey(t => t.ClientId)
                .OnDelete(DeleteBehavior.Restrict);
            #endregion

            #region Properties
            // Configure TransactionStatus enum to be stored as string
            builder.Entity<Transaction>()
                .Property(t => t.Status)
                .HasConversion<string>();
            #endregion
        }
    }
}
