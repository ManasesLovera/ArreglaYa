using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Application.IOC.Mapper
{
    public static class MapperServiceExtensions
    {
        public static IServiceCollection AddMapperServices(this IServiceCollection services)
        {
            // Configure AutoMapper
            // It will scan the assembly for profiles and register them.
            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            return services;
        }
    }
}
