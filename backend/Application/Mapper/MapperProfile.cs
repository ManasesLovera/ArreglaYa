using Application.DTOs.Admin;
using AutoMapper;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Mapper
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            #region Admin
            CreateMap<Admin, AdminDTos>();
            CreateMap<AdminDTos, Admin>();

            CreateMap<Admin, RegisterResponse>();
            CreateMap<RegisterResponse, Admin>();
            #endregion
        }

    }
}
