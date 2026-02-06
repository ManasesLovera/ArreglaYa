using Application.DTOs.User;
using AutoMapper;
using Domain.Models;

namespace Application.Mapper
{
    /// <summary>
    /// AutoMapper profile for user-related mappings.
    /// Configures mappings between user entities and DTOs.
    /// </summary>
    public class UserMapper : Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserMapper"/> class.
        /// Configures all user-related entity to DTO mappings.
        /// </summary>
        public UserMapper()
        {
            // Map CreateUserRequest to ApplicationUser entity
            CreateMap<CreateUserRequest, ApplicationUser>();

            // Map ApplicationUser entity to UserResponse DTO
            // Roles are populated separately via UserManager
            CreateMap<ApplicationUser, UserResponse>()
                .ForMember(dest => dest.Roles, opt => opt.Ignore());
        }
    }
}
