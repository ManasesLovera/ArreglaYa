using Application.DTOs.Account;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{

    public class BaseController : ControllerBase
    {
        protected readonly IValidator<RegisterEntityRequest> _validator;

        public BaseController(IValidator<RegisterEntityRequest> validator)
        {
            _validator = validator;
        }
    }
}
