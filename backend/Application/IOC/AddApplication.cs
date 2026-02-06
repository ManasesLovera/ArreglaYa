using Application.Interfaces.Services;
using Application.Services;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Application.IOC
{
    /// <summary>
    /// Extension methods for configuring Application layer services.
    /// </summary>
    public static class AddApplication
    {
        /// <summary>
        /// Adds Application layer services to the dependency injection container.
        /// Registers AutoMapper, authentication services, and user services.
        /// </summary>
        /// <param name="services">The service collection.</param>
        /// <returns>The service collection for chaining.</returns>
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            // Configure AutoMapper
            // Scans the assembly for mapping profiles and registers them
            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            // Register application services
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IUserService, UserService>();

            return services;
        }
    }
}
