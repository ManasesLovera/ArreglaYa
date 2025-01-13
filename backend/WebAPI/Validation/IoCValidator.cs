using Application.DTOs.Admin;
using Application.DTOs.Company;
using FluentValidation;
using WebAPI.Validation.Admin;
using WebAPI.Validation.Company;

namespace WebAPI.Validation
{
    public static class IoCValidator
    {
        public static void AddValidators(this IServiceCollection services)
        {
            services.AddScoped<IValidator<RegisterRequest>, CreateAdminValidator>();
            services.AddScoped<IValidator<CreateCompanyRequest>, CreateCompanyValidator>();
        }
    }
}
