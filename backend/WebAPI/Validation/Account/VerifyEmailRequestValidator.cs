using Application.DTOs.Account;
using FluentValidation;

namespace WebAPI.Validation.Account
{
    public class VerifyEmailRequestValidator : AbstractValidator<VerifyEmailRequest>
    {
        public VerifyEmailRequestValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .MaximumLength(100).WithMessage("{PropertyName} cannot exceed 100 characters.");

            RuleFor(x => x.Token)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .MaximumLength(500).WithMessage("{PropertyName} cannot exceed 500 characters.");
        }
    }
}
