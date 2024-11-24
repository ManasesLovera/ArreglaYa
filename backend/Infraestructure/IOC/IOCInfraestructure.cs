using Application.Interfaces.Repository;
using Domain.Models;
using Infraestructure.Data;
using Infraestructure.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructure.IOC
{
    public static class IOCInfraestructure
    {
        public static void AddPersistence(this IServiceCollection services, IConfiguration configuration)
        {
            #region Context
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlite(configuration.GetConnectionString("SQLiteConnection"), b => b.MigrationsAssembly("Infraestructure.IOC"));
            });
            #endregion

            #region Identity
            services.AddIdentity<Admin, IdentityRole>()
                     .AddEntityFrameworkStores<ApplicationDbContext>()
                     .AddDefaultTokenProviders();
            #endregion

            #region Repositories
            services.AddTransient<IAdminRepository, AdminRepository>();
            services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            #endregion
        }
    }
}
