using Application.DTOs.Account;
using AutoMapper;
using Domain.Models;

namespace Application.Mapper
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            #region Admin
            CreateMap<Admin, RegisterEntityResponse>();
            CreateMap<RegisterEntityResponse, Admin>();

            CreateMap<Admin, RegisterEntityResponse>();
            CreateMap<RegisterEntityResponse, Admin>();
            #endregion

            #region Client
            CreateMap<Client, RegisterEntityResponse>();
            CreateMap<RegisterEntityResponse, Client>();

            CreateMap<Client, RegisterEntityResponse>();
            CreateMap<RegisterEntityResponse, Client>();
            #endregion
        }

    }
}
