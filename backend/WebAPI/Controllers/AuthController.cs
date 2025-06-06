using Application.DTOs.Account; // LoginRequest, RegisterRequest, VerifyEmailRequest
using Application.DTOs.User;    // UserResponse
using Application.Interfaces;   // IAuthService, ServiceResponse<T>, LoginServiceResponse, RegisterServiceResponse
using AutoMapper;               // IMapper (if BaseController or direct mapping is still used)
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http; // For CookieOptions
using Microsoft.AspNetCore.Mvc;
using System; // For DateTime
using System.Linq; // For validation.ToDictionary()
using System.Security.Claims;
using System.Threading.Tasks;
// We no longer need UserManager, SignInManager, JwtTokenGenerator directly in the controller.
// We also don't need Domain.Models or Microsoft.EntityFrameworkCore here.

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : BaseController // Assuming BaseController provides IMapper or other utilities
    {
        private readonly IAuthService _authService;
        // private readonly ILogger<AuthController> _logger; // Consider adding logger if not in BaseController

        public AuthController(IAuthService authService, IMapper mapper /*, ILogger<AuthController> logger */) : base(mapper)
        {
            _authService = authService;
            // _logger = logger;
        }

        private void SetTokenCookies(string accessToken, string refreshToken)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true, // Should be true in production; ensure your app is served over HTTPS
                SameSite = SameSiteMode.None, // Or SameSiteMode.Lax / SameSiteMode.Strict depending on your needs
                Expires = DateTime.UtcNow.AddDays(7) // Refresh token expiry
            };

            // Access token cookie (shorter expiry)
            var accessTokenCookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = DateTime.UtcNow.AddMinutes(60) // Example: Access token expiry
            };

            Response.Cookies.Append("AccessToken", accessToken, accessTokenCookieOptions);
            Response.Cookies.Append("RefreshToken", refreshToken, cookieOptions); // Refresh token uses longer expiry
        }

        private void DeleteTokenCookies()
        {
            var cookieOptions = new CookieOptions { Secure = true, SameSite = SameSiteMode.None, HttpOnly = true };
            Response.Cookies.Delete("AccessToken", cookieOptions);
            Response.Cookies.Delete("RefreshToken", cookieOptions);
        }

        [Authorize]
        [HttpGet("me")]
        public async Task<IActionResult> Me()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(new { Message = "User identifier not found in token." });
            }

            var serviceResponse = await _authService.GetCurrentUserAsync(userId);

            if (!serviceResponse.Success)
            {
                // Consider specific status codes based on serviceResponse.ErrorMessage
                if (serviceResponse.ErrorMessage != null && serviceResponse.ErrorMessage.Contains("User not found"))
                    return NotFound(new { Message = serviceResponse.ErrorMessage });
                return BadRequest(new { Message = serviceResponse.ErrorMessage });
            }
            return Ok(serviceResponse.Data); // Data is UserResponse
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            if (!ModelState.IsValid) // Standard model validation
            {
                return BadRequest(ModelState);
            }

            var serviceResponse = await _authService.LoginAsync(request);

            if (!serviceResponse.Success)
            {
                return Unauthorized(new { Message = serviceResponse.ErrorMessage });
            }

            SetTokenCookies(serviceResponse.AccessToken, serviceResponse.RefreshToken);
            return Ok(new { User = serviceResponse.User });
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken()
        {
            var refreshTokenFromCookie = Request.Cookies["RefreshToken"];
            if (string.IsNullOrEmpty(refreshTokenFromCookie))
            {
                 return Unauthorized(new { Message = "Refresh token is missing." });
            }

            var serviceResponse = await _authService.RefreshTokenAsync(refreshTokenFromCookie);

            if (!serviceResponse.Success)
            {
                return Unauthorized(new { Message = serviceResponse.ErrorMessage });
            }

            SetTokenCookies(serviceResponse.AccessToken, serviceResponse.RefreshToken);
            return Ok(serviceResponse.User); // User is UserResponse from LoginServiceResponse
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            var refreshTokenFromCookie = Request.Cookies["RefreshToken"];
            await _authService.LogoutAsync(refreshTokenFromCookie);

            DeleteTokenCookies();
            return Ok(new { Message = "Logged out successfully." });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(
            [FromBody] RegisterRequest request,
            [FromServices] IValidator<RegisterRequest> validator)
        {
            var validationResult = await validator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.ToDictionary());
            }

            var serviceResponse = await _authService.RegisterAsync(request);

            if (!serviceResponse.Success)
            {
                object errorResponse = new { Message = serviceResponse.ErrorMessage, Errors = serviceResponse.Errors };
                if (serviceResponse.ErrorMessage != null && serviceResponse.ErrorMessage.ToLower().Contains("email is already in use")) {
                    return Conflict(errorResponse);
                }
                return BadRequest(errorResponse);
            }

            return Ok(new {
                Message = "Registration successful. Please check your email to confirm your account.",
                User = serviceResponse.User, // Return user details
                ConfirmationToken = serviceResponse.ConfirmationToken
            });
        }

        [HttpPost("verify-email")]
        public async Task<IActionResult> VerifyEmail(
            [FromBody] VerifyEmailRequest request,
            [FromServices] IValidator<VerifyEmailRequest> validator)
        {
            var validationResult = await validator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.ToDictionary());
            }

            var serviceResponse = await _authService.VerifyEmailAsync(request);

            if (!serviceResponse.Success)
            {
                object errorResponse = new { Message = serviceResponse.ErrorMessage, Errors = serviceResponse.Errors };
                if (serviceResponse.ErrorMessage != null && serviceResponse.ErrorMessage.ToLower().Contains("user not found")) {
                     return NotFound(errorResponse);
                }
                return BadRequest(errorResponse);
            }

            // serviceResponse.Data is "Email confirmed successfully."
            return Ok(new { Message = serviceResponse.Data });
        }
    }
}
