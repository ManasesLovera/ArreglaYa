using Application.Interfaces.Repository;
using Domain.Models;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.IOC
{
    /// <summary>
    /// Extension methods for configuring Infrastructure layer services.
    /// </summary>
    public static class IOCInfrastructure
    {
        /// <summary>
        /// Adds Infrastructure layer persistence services to the dependency injection container.
        /// Configures database context, Identity framework, and repositories.
        /// </summary>
        /// <param name="services">The service collection.</param>
        /// <param name="configuration">The application configuration.</param>
        public static void AddPersistence(this IServiceCollection services, IConfiguration configuration)
        {
            #region Context
            // Configure Entity Framework Core with SQLite database
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlite(configuration.GetConnectionString("SQLiteConnection")));
            #endregion

            #region Identity
            // Configure ASP.NET Core Identity for user authentication and authorization
            services.AddIdentity<ApplicationUser, IdentityRole>()
                     .AddEntityFrameworkStores<ApplicationDbContext>()
                     .AddDefaultTokenProviders();
            #endregion

            #region Repositories
            // Register repository implementations
            services.AddScoped<IUserRepository, UserRepository>();
            #endregion
        }
    }
}
