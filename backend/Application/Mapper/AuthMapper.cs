using Application.DTOs.Account;
using Application.DTOs.User;
using AutoMapper;
using Domain.Models;

namespace Application.Mapper
{
    /// <summary>
    /// AutoMapper profile for authentication-related mappings.
    /// Configures mappings between authentication DTOs and entities.
    /// </summary>
    public sealed class AuthMapper : Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AuthMapper"/> class.
        /// Configures all authentication-related mappings.
        /// </summary>
        public AuthMapper()
        {
            // Map RegisterRequest to ApplicationUser entity
            CreateMap<RegisterRequest, ApplicationUser>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Email));

            // Map ApplicationUser to UserResponse
            CreateMap<ApplicationUser, UserResponse>()
                .ForMember(dest => dest.Roles, opt => opt.Ignore());
        }
    }
}
