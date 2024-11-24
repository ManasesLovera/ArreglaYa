using Application.Interfaces;
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
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlite(configuration.GetConnectionString("SQLiteConnection"), b => b.MigrationsAssembly("Infraestructure.IOC"));
            });

            services.AddIdentity<IdentityUser, IdentityRole>()
                     .AddEntityFrameworkStores<ApplicationDbContext>()
                     .AddDefaultTokenProviders();

            services.AddScoped<ICompanyRepository,CompanyRepository>();
        }
    }
}
