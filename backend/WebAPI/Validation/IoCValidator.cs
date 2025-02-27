using Application.DTOs.Account;
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
            services.AddScoped<IValidator<RegisterEntityRequest>, CreateAdminValidator>();
            services.AddScoped<IValidator<CreateCompanyRequest>, CreateCompanyValidator>();
        }
    }
}
