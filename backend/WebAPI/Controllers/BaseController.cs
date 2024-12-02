using Application.DTOs.Admin;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
  
    public class BaseController : ControllerBase
    {
        protected readonly IValidator<RegisterRequest> _validator;

        public BaseController(IValidator<RegisterRequest> validator)
        {
            _validator = validator;
        }
    }
}
