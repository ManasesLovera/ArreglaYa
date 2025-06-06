using Application.DTOs.Account;
using Application.DTOs.User;
using Application.Interfaces;
using AutoMapper;
using Domain.Models; // Assuming ApplicationUser is here
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore; // For FirstOrDefaultAsync on UserManager.Users
using System;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Auth.Jwt; // Assuming JwtTokenGenerator is here

namespace Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly JwtTokenGenerator _tokenGenerator; // Using concrete class as per user's AuthController
        private readonly IMapper _mapper;

        public AuthService(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            JwtTokenGenerator tokenGenerator,
            IMapper mapper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenGenerator = tokenGenerator;
            _mapper = mapper;
        }

        public async Task<ServiceResponse<UserResponse>> GetCurrentUserAsync(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                return ServiceResponse<UserResponse>.Fail("User ID cannot be null or empty.");
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return ServiceResponse<UserResponse>.Fail("User not found.");
            }

            var userResponse = _mapper.Map<UserResponse>(user);
            userResponse.Roles = (await _userManager.GetRolesAsync(user)).ToArray();
            return ServiceResponse<UserResponse>.Succeed(userResponse);
        }

        public async Task<LoginServiceResponse> LoginAsync(LoginRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                return LoginServiceResponse.Fail("Invalid email or password.");
            }

            if (!user.EmailConfirmed)
            {
                // Note: Original controller returned Unauthorized("Email not confirmed.")
                // Service layer should provide clear error messages.
                return LoginServiceResponse.Fail("Email not confirmed. Please verify your email address.");
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, lockoutOnFailure: true);
            if (!result.Succeeded)
            {
                // Consider logging failed login attempts here if not handled by SignInManager
                return LoginServiceResponse.Fail("Invalid email or password.");
            }

            var roles = await _userManager.GetRolesAsync(user);
            var accessToken = _tokenGenerator.Generate(user, roles); // Assuming this method exists

            // Refresh token generation and persistence
            var refreshToken = Guid.NewGuid().ToString("N"); // More secure than Guid.NewGuid().ToString()
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
            var updateResult = await _userManager.UpdateAsync(user);

            if (!updateResult.Succeeded)
            {
                // Handle error saving refresh token
                return LoginServiceResponse.Fail("Failed to update user session. Please try again.", updateResult.Errors.Select(e => e.Description).ToArray());
            }

            var userResponse = _mapper.Map<UserResponse>(user);
            userResponse.Roles = roles.ToArray(); // roles already fetched

            return LoginServiceResponse.Succeed(userResponse, accessToken, refreshToken);
        }

        public async Task<LoginServiceResponse> RefreshTokenAsync(string currentRefreshToken)
        {
            if (string.IsNullOrEmpty(currentRefreshToken))
            {
                return LoginServiceResponse.Fail("Refresh token is missing.");
            }

            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.RefreshToken == currentRefreshToken);

            if (user == null || user.RefreshTokenExpiryTime < DateTime.UtcNow)
            {
                return LoginServiceResponse.Fail("Invalid or expired refresh token.");
            }

            var roles = await _userManager.GetRolesAsync(user);
            var newAccessToken = _tokenGenerator.Generate(user, roles);

            // Rotate refresh token
            var newRefreshToken = Guid.NewGuid().ToString("N");
            user.RefreshToken = newRefreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
            var updateResult = await _userManager.UpdateAsync(user);

            if (!updateResult.Succeeded)
            {
                return LoginServiceResponse.Fail("Failed to update user session.", updateResult.Errors.Select(e => e.Description).ToArray());
            }

            var userResponse = _mapper.Map<UserResponse>(user);
            userResponse.Roles = roles.ToArray(); // roles already fetched

            return LoginServiceResponse.Succeed(userResponse, newAccessToken, newRefreshToken);
        }

        public async Task<ServiceResponse<string>> LogoutAsync(string currentRefreshToken)
        {
            if (!string.IsNullOrEmpty(currentRefreshToken))
            {
                var user = await _userManager.Users.FirstOrDefaultAsync(u => u.RefreshToken == currentRefreshToken);
                if (user != null)
                {
                    user.RefreshToken = null;
                    user.RefreshTokenExpiryTime = DateTime.MinValue; // Or DateTime.UtcNow
                    await _userManager.UpdateAsync(user);
                }
            }
            // Controller handles cookie deletion. Service confirms logout.
            return ServiceResponse<string>.Succeed("Logged out successfully.");
        }

        public async Task<RegisterServiceResponse> RegisterAsync(RegisterRequest request)
        {
            // FluentValidation is handled in controller in the original code.
            // Service can also do validation or assume valid DTO.
            // For now, assume DTO is valid as per original controller structure.

            var userExists = await _userManager.FindByEmailAsync(request.Email);
            if (userExists != null)
            {
                // Original controller returns Conflict("This email is in used")
                return RegisterServiceResponse.Fail("This email is already in use.");
            }

            var user = _mapper.Map<ApplicationUser>(request);
            // UserName might need to be set explicitly if not mapped from Email or FullName by AutoMapper
            // For Identity, UserName is often required and unique.
            if (string.IsNullOrEmpty(user.UserName))
            {
                user.UserName = request.Email; // Common practice
            }


            var result = await _userManager.CreateAsync(user, request.Password);
            if (!result.Succeeded)
            {
                return RegisterServiceResponse.Fail("User creation failed.", result.Errors.Select(e => e.Description).ToArray());
            }

            var roles = await _userManager.GetRolesAsync(user); // Should be empty for new user unless default roles are assigned
            var userResponse = _mapper.Map<UserResponse>(user);
            userResponse.Roles = roles.ToArray();

            // Email confirmation token
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            // The service returns the token; controller can decide to send email or return it in response (as in original).
            return RegisterServiceResponse.Succeed(userResponse, token);
        }

        public async Task<ServiceResponse<string>> VerifyEmailAsync(VerifyEmailRequest request)
        {
            // FluentValidation is handled in controller in the original code.
            var user = await _userManager.FindByIdAsync(request.UserId);
            if (user == null)
            {
                return ServiceResponse<string>.Fail("User not found.");
            }

            var result = await _userManager.ConfirmEmailAsync(user, request.Token);
            if (!result.Succeeded)
            {
                return ServiceResponse<string>.Fail("Email confirmation failed.", result.Errors.Select(e => e.Description).ToArray());
            }
            return ServiceResponse<string>.Succeed("Email confirmed successfully.");
        }
    }
}
