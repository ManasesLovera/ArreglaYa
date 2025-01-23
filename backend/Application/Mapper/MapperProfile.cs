using Application.DTOs.Admin;
using Application.DTOs.Client;
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
            CreateMap<BaseUser, AdminDTos>();
            CreateMap<AdminDTos, BaseUser>();

            CreateMap<BaseUser, RegisterResponse>();
            CreateMap<RegisterResponse, BaseUser>();
            #endregion

            #region Client
            CreateMap<BaseUser, ClientDto>();
            CreateMap<ClientDto, BaseUser>();

            CreateMap<BaseUser, RegisterResponse>();
            CreateMap<RegisterResponse, BaseUser>();
            #endregion
        }

    }
}
