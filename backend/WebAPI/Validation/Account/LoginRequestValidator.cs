using Application.DTOs.Account;
using FluentValidation;

namespace WebAPI.Validation.Account
{
    public class LoginRequestValidator :AbstractValidator<LoginRequest>
    {
        public LoginRequestValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .EmailAddress().WithMessage("Invalid {PropertyName} format.")
                .MaximumLength(254).WithMessage("{PropertyName} cannot exceed 254 characters.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .MinimumLength(8).WithMessage("{PropertyName} must be at least 8 characters.")
                .MaximumLength(50).WithMessage("{PropertyName} cannot exceed 50 characters.");
        }
    }
}
