using Application.DTOs.Common;
using Application.DTOs.User;
using FluentValidation;
using WebAPI.Validation.User;

namespace WebAPI.Validation
{
    public static class IoCValidator
    {
        public static void AddValidators(this IServiceCollection services)
        {
            services.AddScoped<IValidator<CreateUserRequest>, CreateUserValidator>();
            services.AddScoped<IValidator<UpdatePasswordRequest>, UpdatePasswordRequestValidator>();
        }
    }
}
