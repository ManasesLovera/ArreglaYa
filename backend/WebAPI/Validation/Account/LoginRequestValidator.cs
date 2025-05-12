using FluentValidation;
using Microsoft.AspNetCore.Identity.Data;

namespace WebAPI.Validation.Account
{
    public class LoginRequestValidator :AbstractValidator<LoginRequest>
    {
        public LoginRequestValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().NotNull().WithMessage("Email address is missing")
                .EmailAddress().WithMessage("Email is not valid");
            RuleFor(x => x.Password)
                .NotEmpty().NotNull().WithMessage("Password cannot be empty")
                .MinimumLength(7).WithMessage("Password length must be 8 or higher");             
        }
    }
}
