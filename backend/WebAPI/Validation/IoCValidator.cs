<<<<<<< HEAD
﻿using Application.DTOs.Common;
using Application.DTOs.User;
=======
﻿using Application.DTOs.Account;
using Application.DTOs.Company;
>>>>>>> dev
using FluentValidation;
using WebAPI.Validation.User;

namespace WebAPI.Validation
{
    public static class IoCValidator
    {
        public static void AddValidators(this IServiceCollection services)
        {
<<<<<<< HEAD
            services.AddScoped<IValidator<CreateUserRequest>, CreateUserValidator>();
            services.AddScoped<IValidator<UpdatePasswordRequest>, UpdatePasswordRequestValidator>();
=======
            services.AddScoped<IValidator<RegisterEntityRequest>, CreateAdminValidator>();
            services.AddScoped<IValidator<CreateCompanyRequest>, CreateCompanyValidator>();
>>>>>>> dev
        }
    }
}
