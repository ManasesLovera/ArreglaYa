using Application.DTOs.Account;
using WebAPI.Validation.Common;
using FluentValidation;

namespace WebAPI.Validation.Account
{
    public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
    {
        public RegisterRequestValidator()
        {
            RuleFor(x => x.FullName)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .MinimumLength(8).WithMessage("{PropertyName} must be at least 8 characters.")
                .MaximumLength(100).WithMessage("{PropertyName} cannot exceed 100 characters.");

            RuleFor(x => x.UserName)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .MinimumLength(4).WithMessage("{PropertyName} must be at least 4 characters.")
                .MaximumLength(30).WithMessage("{PropertyName} cannot exceed 30 characters.")
                .Matches(CommonValidators.UserNamePattern).WithMessage("{PropertyName} can only contain letters, numbers, and _ . -");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .EmailAddress().WithMessage("Invalid {PropertyName} format.");

            RuleFor(x => x.PhoneNumber)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .Must(CommonValidators.BeValidPhoneNumber)
                .WithMessage("{PropertyName} must be a valid phone number. Accepted formats include: '+1 829 546 6017', '8295466017', '(829) 546-6017', or '829-546-6017'. Only numeric digits will be validated, ignoring spaces, dashes, or parentheses.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .MinimumLength(8).WithMessage("{PropertyName} must be at least 8 characters.")
                .MaximumLength(50).WithMessage("{PropertyName} cannot exceed 50 characters.")
                .Must(CommonValidators.ContainsUppercase).WithMessage("{PropertyName} must contain at least one uppercase letter.")
                .Must(CommonValidators.ContainsLowercase).WithMessage("{PropertyName} must contain at least one lowercase letter.")
                .Must(CommonValidators.ContainsDigit).WithMessage("{PropertyName} must contain at least one digit.")
                .Must(CommonValidators.ContainsSpecialCharacter).WithMessage("{PropertyName} must contain at least one special character.");

            RuleFor(x => x.PasswordConfirmation)
                .NotEmpty().WithMessage("Password confirmation is required.")
                .Equal(x => x.Password).WithMessage("Passwords do not match.");
        }

        
    }
}
