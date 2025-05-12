using Application.DTOs.Account;
using FluentValidation;

namespace WebAPI.Validation.Account
{
    public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
    {
        public RegisterRequestValidator()
        {
            RuleFor(x => x.FullName)
                .NotEmpty().NotNull().WithMessage("{PropertyName} cannot be null")
                .MinimumLength(8).WithMessage("{PropertyName} cannot has under 8 characters");
        }
    }
}
