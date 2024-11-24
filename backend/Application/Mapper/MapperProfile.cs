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
            CreateMap<Admin, UpdateAdminDTos>()
                            .ReverseMap();
            CreateMap<Admin, AdminDTos>()
                            .ReverseMap(); 
            #endregion
        }

    }
}
