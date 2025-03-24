using Application.DTOs.User;
using FluentValidation;

namespace WebAPI.Validation.User
{
    public class CreateUserValidator : AbstractValidator<CreateUserRequest>
    {
        public CreateUserValidator()
        {
            RuleFor(x => x.Fullname)
                .NotNull().NotEmpty()
                .WithMessage("{PropertyName} can't be null or empty.")
                .MinimumLength(5)
                .WithMessage("{PropertyName} length must be at least 5 characters.");

            RuleFor(x => x.Username)
                .NotNull().NotEmpty()
                .WithMessage("{PropertyName} can't be null or empty.")
                .MinimumLength(3)
                .WithMessage("{PropertyName} can't have under 3 characters.");

            RuleFor(x => x.Email)
                .NotNull().NotEmpty()
                .WithMessage("{PropertyName} can't be null or empty.")
                .EmailAddress()
                .WithMessage("{PropertyName} must be a valid email address.");

            RuleFor(x => x.Password)
                .NotNull().NotEmpty()
                .WithMessage("{PropertyName} can't be null or empty.")
                .MinimumLength(8)
                .WithMessage("{PropertyName} should be at least 8 characters");
        }
    }
}
