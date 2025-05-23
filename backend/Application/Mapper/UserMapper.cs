using Application.DTOs.User;
using AutoMapper;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Mapper
{
    public class UserMapper : Profile
    {
        public UserMapper()
        {
            CreateMap<CreateUserRequest, ApplicationUser>();
            CreateMap<ApplicationUser, UserResponse>()
                .ForMember(dest => dest.Roles, opt => opt.Ignore());
        }
    }
}
