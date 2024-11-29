using Application.DTOs.Admin;
using FluentValidation;

namespace WebAPI.Validation.Admin
{
    public class CreateAdminValidator : AbstractValidator<RegisterRequest>
    {
        public CreateAdminValidator()
        {
            RuleFor(x => x.Email).EmailAddress().WithMessage("It is not an email address");
            RuleFor(x => x.Email).NotNull().WithMessage("Can't not null {PropertyName}");
            RuleFor(x => x.Username).NotNull().WithMessage("Can't not null {PropertyName}");
            RuleFor(x => x.Password).NotNull().WithMessage("Can't not null {PropertyName}");
            RuleFor(x => x.Password).MinimumLength(5).WithMessage("The minimum number of characters is {MinLength}");

        }
    }
}
