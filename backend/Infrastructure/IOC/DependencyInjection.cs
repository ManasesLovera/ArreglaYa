using Infrastructure.IOC.Data;
using Infrastructure.IOC.Identity;
using Infrastructure.IOC.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.IOC
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            // Add Data Services (DbContext)
            services.AddDataServices(configuration);

            // Add Identity Services
            services.AddIdentityServices();

            // Add Repository Services
            services.AddRepositoryServices();

            return services;
        }
    }
}
