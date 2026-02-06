using Application.Interfaces.Services;
using Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Application.IOC.Services
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            // Register application services
            services.AddScoped<IAuthService, AuthService>();
            // Note: ISmtpEmailService can be added here when needed
            // services.AddScoped<IEmailService, SmtpEmailService>();

            return services;
        }
    }
}
