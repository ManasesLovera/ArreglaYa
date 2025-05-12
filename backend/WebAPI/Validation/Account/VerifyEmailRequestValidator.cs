using Application.DTOs.Account;
using FluentValidation;

namespace WebAPI.Validation.Account
{
    public class VerifyEmailRequestValidator : AbstractValidator<VerifyEmailRequest>
    {
        public VerifyEmailRequestValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty().NotNull().WithMessage("{PropertyName} cannot be null");

            RuleFor(x => x.Token)
                .NotEmpty().NotNull().WithMessage("{PropertyName} cannot be null");
        }
    }
}
