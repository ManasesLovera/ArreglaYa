<<<<<<< HEAD
﻿using Application.DTOs.User;
using AutoMapper;
=======
﻿using Application.DTOs.Account;
>>>>>>> dev
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{

    public class BaseController : ControllerBase
    {
<<<<<<< HEAD
        protected readonly IMapper _mapper;

        public BaseController(IMapper mapper)
=======
        protected readonly IValidator<RegisterEntityRequest> _validator;

        public BaseController(IValidator<RegisterEntityRequest> validator)
>>>>>>> dev
        {
            _mapper = mapper;
        }
    }
}
