using Application.DTOs.Account;
using Application.DTOs.Common;
using Application.DTOs.User;
using FluentValidation;
using Microsoft.AspNetCore.Identity.Data;
using WebAPI.Validation.Account;
using WebAPI.Validation.User;

namespace WebAPI.Validation
{
    public static class IoCValidator
    {
        public static void AddValidators(this IServiceCollection services)
        {
            services.AddScoped<IValidator<CreateUserRequest>, CreateUserValidator>();
            services.AddScoped<IValidator<UpdatePasswordRequest>, UpdatePasswordRequestValidator>();
            services.AddScoped<IValidator<LoginRequest>, LoginRequestValidator>();
            services.AddScoped<IValidator<RefreshTokenRequest>, RefreshTokenRequestValidator>();
            services.AddScoped<IValidator<VerifyEmailRequest>, VerifyEmailRequestValidator>();
        }
    }
}
