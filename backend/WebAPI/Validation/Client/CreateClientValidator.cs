using Application.DTOs.Account;
using FluentValidation;

namespace WebAPI.Validation.Client
{
    public class CreateClientValidator : AbstractValidator<RegisterEntityRequest>
    {
        public CreateClientValidator()
        {

            RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage("{PropertyName} can't be null or empty")
            .EmailAddress()
            .WithMessage("{PropertyName} is not a valid email address");
            RuleFor(x => x.Username).NotNull().WithMessage("Can't not null {PropertyName}");
            RuleFor(x => x.Password).NotNull().WithMessage("Can't not null {PropertyName}");
            RuleFor(x => x.Password).MinimumLength(5).WithMessage("The minimum number of characters is {MinLength}");

        }
    }
}
