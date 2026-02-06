using Microsoft.Extensions.DependencyInjection;
using WebAPI.Auth.Jwt;
using WebAPI.Configuration;
using WebAPI.Validation;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Builder;

namespace WebAPI.IOC
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddWebAPILayer(
            this IServiceCollection services, 
            IConfiguration configuration,
            WebApplicationBuilder builder)
        {
            // Add CORS Configuration
            services.AddCustomCors(builder.Environment);

            // Add Swagger Configuration
            services.AddCustomSwagger();

            // Add JWT Authentication
            services.AddJwtAuthentication(configuration);

            // Add Validators
            services.AddValidators();

            return services;
        }
    }
}
