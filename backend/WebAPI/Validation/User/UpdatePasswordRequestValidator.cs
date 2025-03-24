using Application.DTOs.Common;
using FluentValidation;

namespace WebAPI.Validation.User
{
    public class UpdatePasswordRequestValidator : AbstractValidator<UpdatePasswordRequest>
    {
        public UpdatePasswordRequestValidator()
        {
            
        }
    }
}
