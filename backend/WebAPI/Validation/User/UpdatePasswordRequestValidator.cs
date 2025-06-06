using Application.DTOs.Common;
using FluentValidation;
using WebAPI.Validation.Common;

namespace WebAPI.Validation.User
{
    public class UpdatePasswordRequestValidator : AbstractValidator<UpdatePasswordRequest>
    {
        public UpdatePasswordRequestValidator()
        {
            RuleFor(x => x.CurrentPassword)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .MinimumLength(8).WithMessage("{PropertyName} must be at least 8 characters.")
                .MaximumLength(50).WithMessage("{PropertyName} cannot exceed 50 characters.");

            RuleFor(x => x.NewPassword)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .MinimumLength(8).WithMessage("{PropertyName} must be at least 8 characters.")
                .MaximumLength(50).WithMessage("{PropertyName} cannot exceed 50 characters.")
                .Must(CommonValidators.ContainsUppercase).WithMessage("{PropertyName} must contain at least one uppercase letter.")
                .Must(CommonValidators.ContainsLowercase).WithMessage("{PropertyName} must contain at least one lowercase letter.")
                .Must(CommonValidators.ContainsDigit).WithMessage("{PropertyName} must contain at least one digit.")
                .Must(CommonValidators.ContainsSpecialCharacter).WithMessage("{PropertyName} must contain at least one special character.")
                .NotEqual(x => x.CurrentPassword).WithMessage("{PropertyName} must be different from current password.");
        }
    }
}
