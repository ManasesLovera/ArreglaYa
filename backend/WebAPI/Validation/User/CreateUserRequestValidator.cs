using Application.DTOs.User;
using FluentValidation;
using WebAPI.Validation.Common;

namespace WebAPI.Validation.User
{
    public class CreateUserRequestValidator : AbstractValidator<CreateUserRequest>
    {
        public CreateUserRequestValidator()
        {
            RuleFor(x => x.Username)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .MinimumLength(4).WithMessage("{PropertyName} must be at least 4 characters.")
                .MaximumLength(30).WithMessage("{PropertyName} cannot exceed 30 characters.")
                .Matches(CommonValidators.UserNamePattern).WithMessage("{PropertyName} can only contain letters, numbers, and _ . -");

            RuleFor(x => x.Fullname)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .MinimumLength(8).WithMessage("{PropertyName} must be at least 8 characters.")
                .MaximumLength(100).WithMessage("{PropertyName} cannot exceed 100 characters.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .EmailAddress().WithMessage("Invalid {PropertyName} format.")
                .MaximumLength(254).WithMessage("{PropertyName} cannot exceed 254 characters.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .MinimumLength(8).WithMessage("{PropertyName} must be at least 8 characters.")
                .MaximumLength(50).WithMessage("{PropertyName} cannot exceed 50 characters.")
                .Must(CommonValidators.ContainsUppercase).WithMessage("{PropertyName} must contain at least one uppercase letter.")
                .Must(CommonValidators.ContainsLowercase).WithMessage("{PropertyName} must contain at least one lowercase letter.")
                .Must(CommonValidators.ContainsDigit).WithMessage("{PropertyName} must contain at least one digit.")
                .Must(CommonValidators.ContainsSpecialCharacter).WithMessage("{PropertyName} must contain at least one special character.");
        }
    }
}
