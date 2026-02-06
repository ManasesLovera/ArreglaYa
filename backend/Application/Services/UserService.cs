using Application.DTOs.Common;
using Application.DTOs.User;
using Application.Interfaces.Repository;
using Application.Interfaces.Services;
using AutoMapper;
using Domain.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Services
{
    /// <summary>
    /// Service for managing user operations.
    /// Provides abstraction over Identity UserManager and repository operations.
    /// </summary>
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserService"/> class.
        /// </summary>
        /// <param name="userRepository">The user repository for data access.</param>
        /// <param name="userManager">The ASP.NET Core Identity UserManager.</param>
        /// <param name="mapper">The AutoMapper instance for DTO mapping.</param>
        public UserService(
            IUserRepository userRepository,
            UserManager<ApplicationUser> userManager,
            IMapper mapper)
        {
            _userRepository = userRepository;
            _userManager = userManager;
            _mapper = mapper;
        }

        /// <inheritdoc/>
        public async Task<PaginatedResponse<UserResponse>> GetPaginatedUsersAsync(int pageIndex, int pageSize)
        {
            var users = await _userRepository.GetPaginatedAsync(pageIndex, pageSize);
            var totalRecords = await _userRepository.GetTotalCountAsync();

            var userResponses = new List<UserResponse>();

            foreach (var user in users)
            {
                var userResponse = _mapper.Map<UserResponse>(user);
                userResponse.Roles = (await _userManager.GetRolesAsync(user)).ToArray();
                userResponses.Add(userResponse);
            }

            return new PaginatedResponse<UserResponse>(
                userResponses,
                totalRecords,
                pageIndex,
                pageSize
            );
        }

        /// <inheritdoc/>
        public async Task<UserResult> GetUserByIdAsync(string id)
        {
            var user = await _userRepository.GetByIdAsync(id);

            if (user == null)
            {
                return new UserResult(false, "User was not found");
            }

            var userResponse = _mapper.Map<UserResponse>(user);
            userResponse.Roles = (await _userManager.GetRolesAsync(user)).ToArray();

            return new UserResult(true, User: userResponse);
        }

        /// <inheritdoc/>
        public async Task<UserResult> CreateUserAsync(CreateUserRequest request, string role = "Client")
        {
            // Check if username or email already exists
            var existingUserByName = await _userManager.FindByNameAsync(request.Username);
            if (existingUserByName != null)
            {
                return new UserResult(false, $"Username '{request.Username}' is already taken.");
            }

            var existingUserByEmail = await _userManager.FindByEmailAsync(request.Email);
            if (existingUserByEmail != null)
            {
                return new UserResult(false, $"Email '{request.Email}' is already registered.");
            }

            var user = _mapper.Map<ApplicationUser>(request);

            var result = await _userManager.CreateAsync(user, request.Password);
            if (!result.Succeeded)
            {
                var errors = string.Join("; ", result.Errors.Select(e => e.Description));
                return new UserResult(false, errors);
            }

            await _userManager.AddToRoleAsync(user, role);

            var userResponse = _mapper.Map<UserResponse>(user);
            userResponse.Roles = new[] { role };

            return new UserResult(true, "User created successfully", userResponse);
        }

        /// <inheritdoc/>
        public async Task<bool> DeleteUserAsync(string id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
            {
                return false;
            }

            await _userRepository.DeleteAsync(user.Id);
            return true;
        }

        /// <inheritdoc/>
        public async Task<(bool Success, string Message)> UpdatePasswordAsync(string userId, string currentPassword, string newPassword)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                return (false, "User not found.");
            }

            var result = await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);
            if (!result.Succeeded)
            {
                var errors = string.Join("; ", result.Errors.Select(e => e.Description));
                return (false, errors);
            }

            return (true, "Password updated successfully.");
        }
    }
}
