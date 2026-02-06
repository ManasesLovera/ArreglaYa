using Application.Interfaces.Services; // Added for IAuthService
using Application.Services; // Added for AuthService
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;
// Assuming SmtpEmailService and ISmtpEmailService are correctly namespaced if they exist
// For example:
// using Application.Interfaces.Services.Email; // If ISmtpEmailService is there
// using Application.Services.Email; // If SmtpEmailService is there

namespace Application.IOC
{
    /// <summary>
    /// DEPRECATED: This class has been replaced by the modular DependencyInjection pattern.
    /// Use Application.IOC.DependencyInjection.AddApplicationLayer() instead.
    /// This class is kept for backward compatibility only.
    /// </summary>
    [Obsolete("Use Application.IOC.DependencyInjection.AddApplicationLayer() instead")]
    public static class AddApplication
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            // Configure AutoMapper
            // It will scan the assembly for profiles and register them.
            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            // Register other services
            // Example: services.AddScoped<ISmtpEmailService, SmtpEmailService>();
            // If SmtpEmailService.cs is in Application/Services, its interface might be in Application/Interfaces/Services
            // Based on ls, SmtpEmailService is in Application/Services. Assuming its interface ISmtpEmailService is in Application/Interfaces/Services.

            // Check if SmtpEmailService and its interface exist and register them if so.
            // The file listing showed: backend/Application/Services/SmtpEmailService.cs
            // and backend/Application/Interfaces/Services/...
            // Let's assume ISmtpEmailService is defined in Application.Interfaces.Services namespace
            // and SmtpEmailService in Application.Services namespace.
            // If these files are not fully implemented or have different namespaces, this might need adjustment.
            // For now, we'll keep the original structure and add our service.

            // If ISmtpEmailService and SmtpEmailService are indeed defined and used:
            // services.AddScoped<ISmtpEmailService, SmtpEmailService>();
            // For now, let's comment this out if it was just an example and not confirmed to be registered.
            // The important part is adding the IAuthService.

            services.AddScoped<IAuthService, AuthService>();

            return services;
        }
    }
}
