using Application.DTOs.Account;
using Application.DTOs.Common;
using Application.DTOs.User;
using FluentValidation;
using WebAPI.Validation.Account;
using WebAPI.Validation.User;

namespace WebAPI.Validation
{
    public static class IoCValidator
    {
        public static void AddValidators(this IServiceCollection services)
        {
            services.AddScoped<IValidator<CreateUserRequest>, CreateUserRequestValidator>();
            services.AddScoped<IValidator<UpdatePasswordRequest>, UpdatePasswordRequestValidator>();
            services.AddScoped<IValidator<LoginRequest>, LoginRequestValidator>();
            services.AddScoped<IValidator<VerifyEmailRequest>, VerifyEmailRequestValidator>();
            services.AddScoped<IValidator<RegisterRequest>, RegisterRequestValidator>();
        }
    }
}
