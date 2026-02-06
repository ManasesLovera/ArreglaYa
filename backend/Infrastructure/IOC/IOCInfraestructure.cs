using Application.Interfaces.Repository;
using Domain.Models;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain;

namespace Infrastructure.IOC
{
    /// <summary>
    /// DEPRECATED: This class has been replaced by the modular DependencyInjection pattern.
    /// Use Infrastructure.IOC.DependencyInjection.AddInfrastructure() instead.
    /// This class is kept for backward compatibility only.
    /// </summary>
    [Obsolete("Use Infrastructure.IOC.DependencyInjection.AddInfrastructure() instead")]
    public static class IOCInfrastructure
    {
        /// <summary>
        /// DEPRECATED: Use Infrastructure.IOC.DependencyInjection.AddInfrastructure() instead.
        /// </summary>
        [Obsolete("Use Infrastructure.IOC.DependencyInjection.AddInfrastructure() instead")]
        public static void AddPersistenceLegacy(this IServiceCollection services, IConfiguration configuration)
        {
            #region Context
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlite(configuration.GetConnectionString("SQLiteConnection")));
            #endregion

            #region Identity
            services.AddIdentity<ApplicationUser, IdentityRole>()
                     .AddEntityFrameworkStores<ApplicationDbContext>()
                     .AddDefaultTokenProviders();
            #endregion

            #region Repositories
            services.AddScoped<IUserRepository, UserRepository>();
            #endregion
        }
    }
}
