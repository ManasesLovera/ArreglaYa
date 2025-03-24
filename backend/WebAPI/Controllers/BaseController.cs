using Application.DTOs.User;
using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
  
    public class BaseController : ControllerBase
    {
        protected readonly IMapper _mapper;

        public BaseController(IMapper mapper)
        {
            _mapper = mapper;
        }
    }
}
