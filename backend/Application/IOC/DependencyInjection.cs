using Application.IOC.Mapper;
using Application.IOC.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Application.IOC
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationLayer(this IServiceCollection services)
        {
            // Add Mapper Services (AutoMapper)
            services.AddMapperServices();

            // Add Application Services
            services.AddApplicationServices();

            return services;
        }
    }
}
