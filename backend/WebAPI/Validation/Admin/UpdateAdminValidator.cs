using Application.DTOs.Admin;
using FluentValidation;

namespace WebAPI.Validations.Admin
{
    public class UpdateAdminValidator : AbstractValidator<UpdateAdminDTos>
    {
        public UpdateAdminValidator()
        {
            RuleFor(x => x.Email).NotNull().WithMessage("Can't not null {PropertyName}");
            
            RuleFor(x => x.Phone).NotNull().WithMessage("Is required");
        }
    }
}
