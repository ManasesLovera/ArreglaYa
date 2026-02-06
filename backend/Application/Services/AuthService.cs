using Application.DTOs.Account;
using Application.DTOs.User;
using Application.Interfaces.Services;
using AutoMapper;
using Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Application.Auth;
using System;
using System.Linq;
using System.Threading.Tasks; // Ensure Task is available

namespace Application.Services
{
    /// <summary>
    /// Provides services for user authentication, registration, and token management.
    /// Implements <see cref="IAuthService"/>.
    /// </summary>
    public class AuthService : IAuthService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly JwtTokenGenerator _tokenGenerator;
        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthService"/> class.
        /// </summary>
        /// <param name="userManager">The ASP.NET Core Identity UserManager for managing user persistence.</param>
        /// <param name="signInManager">The ASP.NET Core Identity SignInManager for handling user sign-ins.</param>
        /// <param name="tokenGenerator">The JWT token generator.</param>
        /// <param name="mapper">The AutoMapper instance for DTO mapping.</param>
        public AuthService(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            JwtTokenGenerator tokenGenerator,
            IMapper mapper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenGenerator = tokenGenerator;
            _mapper = mapper;
        }

        /// <inheritdoc/>
        public async Task<UserResponse> GetCurrentUserAsync(ClaimsPrincipal userPrincipal)
        {
            var userId = userPrincipal.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
                return null;

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return null;

            var userResponse = _mapper.Map<UserResponse>(user);
            userResponse.Roles = (await _userManager.GetRolesAsync(user)).ToArray();
            return userResponse;
        }

        /// <inheritdoc/>
        public async Task<(UserResponse User, string AccessToken, string RefreshToken)> LoginAsync(LoginRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
                throw new UnauthorizedAccessException("Invalid email or password.");

            var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, lockoutOnFailure: true);
            if (!result.Succeeded)
                throw new UnauthorizedAccessException("Invalid email or password.");

            if (!user.EmailConfirmed)
                throw new UnauthorizedAccessException("Email not confirmed.");

            var roles = await _userManager.GetRolesAsync(user);
            var accessToken = _tokenGenerator.Generate(user, roles);
            var refreshToken = Guid.NewGuid().ToString("N");

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
            var updateResult = await _userManager.UpdateAsync(user);
            if(!updateResult.Succeeded)
                throw new Exception($"Failed to update user with refresh token. Errors: {string.Join(", ", updateResult.Errors.Select(e => e.Description))}");

            var userResponse = _mapper.Map<UserResponse>(user);
            userResponse.Roles = roles.ToArray();
            return (userResponse, accessToken, refreshToken);
        }

        /// <inheritdoc/>
        public async Task<(string NewAccessToken, string NewRefreshToken, UserResponse User)> RefreshTokenAsync(string? existingRefreshToken)
        {
            if (string.IsNullOrEmpty(existingRefreshToken))
                throw new ArgumentNullException(nameof(existingRefreshToken), "Refresh token cannot be null or empty.");

            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.RefreshToken == existingRefreshToken);
            if (user == null || user.RefreshTokenExpiryTime < DateTime.UtcNow)
                throw new UnauthorizedAccessException("Invalid or expired refresh token.");

            var roles = await _userManager.GetRolesAsync(user);
            var newAccessToken = _tokenGenerator.Generate(user, roles);
            var newRefreshToken = Guid.NewGuid().ToString("N");

            user.RefreshToken = newRefreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
            var updateResult = await _userManager.UpdateAsync(user);
            if(!updateResult.Succeeded)
                throw new Exception($"Failed to update user with new refresh token. Errors: {string.Join(", ", updateResult.Errors.Select(e => e.Description))}");

            var userResponse = _mapper.Map<UserResponse>(user);
            userResponse.Roles = roles.ToArray();
            return (newAccessToken, newRefreshToken, userResponse);
        }

        /// <inheritdoc/>
        public async Task LogoutAsync(string? existingRefreshToken)
        {
            if (!string.IsNullOrEmpty(existingRefreshToken))
            {
                var user = await _userManager.Users.FirstOrDefaultAsync(u => u.RefreshToken == existingRefreshToken);
                if (user != null)
                {
                    user.RefreshToken = null;
                    user.RefreshTokenExpiryTime = DateTime.MinValue;
                    var updateResult = await _userManager.UpdateAsync(user);
                    if (!updateResult.Succeeded)
                    {
                        // Log errors: Console.WriteLine($"Failed to clear refresh token for user {user.Id}. Errors: {string.Join(", ", updateResult.Errors.Select(e => e.Description))}");
                        // Not throwing to client for logout failures on server-side token clearing.
                    }
                }
            }
        }

        /// <inheritdoc/>
        public async Task<string> RegisterAsync(RegisterRequest request)
        {
            var userExists = await _userManager.FindByEmailAsync(request.Email!);
            if (userExists != null)
                throw new ArgumentException("This email is already in use.");

            var user = new User
            {
                FullName = request.FullName ?? string.Empty,
                UserName = request.UserName ?? request.Email,
                Email = request.Email,
                PhoneNumber = request.PhoneNumber
            };

            var result = await _userManager.CreateAsync(user, request.Password!);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new Exception($"User registration failed: {errors}");
            }
            
            // Assign default "Client" role to newly registered users
            var roleResult = await _userManager.AddToRoleAsync(user, "Client");
            if (!roleResult.Succeeded)
            {
                var errors = string.Join(", ", roleResult.Errors.Select(e => e.Description));
                throw new Exception($"Failed to assign role to user: {errors}");
            }
            
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            return token;
        }

        /// <inheritdoc/>
        public async Task<bool> VerifyEmailAsync(VerifyEmailRequest request)
        {
            var user = await _userManager.FindByIdAsync(request.UserId);
            if (user == null)
                throw new ArgumentException("User not found.");

            var result = await _userManager.ConfirmEmailAsync(user, request.Token);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new Exception($"Email confirmation failed: {errors}");
            }
            return true;
        }
    }
}
