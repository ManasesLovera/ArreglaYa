using Application.DTOs.Account;
using Application.DTOs.User;
using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Application.Interfaces.Services;
using System;
using Microsoft.AspNetCore.Http;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : BaseController
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService, IMapper mapper) : base(mapper)
        {
            _authService = authService;
        }

        [Authorize]
        [HttpGet("me")]
        public async Task<IActionResult> Me()
        {
            var userResponse = await _authService.GetCurrentUserAsync(User);
            if (userResponse == null)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (userId == null) return Unauthorized();
                return NotFound("User not found.");
            }
            return Ok(userResponse);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            try
            {
                var (userResponse, accessToken, refreshToken) = await _authService.LoginAsync(request);
                var cookieOptions = new CookieOptions { HttpOnly = true, Secure = true, SameSite = SameSiteMode.None, Path = "/" };
                Response.Cookies.Append("AccessToken", accessToken, new CookieOptions { Path = cookieOptions.Path, HttpOnly = cookieOptions.HttpOnly, Secure = cookieOptions.Secure, SameSite = cookieOptions.SameSite, Expires = DateTime.UtcNow.AddMinutes(60) });
                Response.Cookies.Append("RefreshToken", refreshToken, new CookieOptions { Path = cookieOptions.Path, HttpOnly = cookieOptions.HttpOnly, Secure = cookieOptions.Secure, SameSite = cookieOptions.SameSite, Expires = DateTime.UtcNow.AddDays(7) });
                return Ok(new { User = userResponse });
            }
            catch (UnauthorizedAccessException ex) { return Unauthorized(ex.Message); }
            catch (Exception ex) { return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred during login."); }
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken()
        {
            var refreshTokenFromCookie = Request.Cookies["RefreshToken"];
            if (string.IsNullOrEmpty(refreshTokenFromCookie)) return Unauthorized("Refresh token is missing.");
            try
            {
                var (newAccessToken, newRefreshToken, userResponse) = await _authService.RefreshTokenAsync(refreshTokenFromCookie);
                var cookieOptions = new CookieOptions { HttpOnly = true, Secure = true, SameSite = SameSiteMode.Strict, Path = "/" };
                Response.Cookies.Append("AccessToken", newAccessToken, new CookieOptions { Path = cookieOptions.Path, HttpOnly = cookieOptions.HttpOnly, Secure = cookieOptions.Secure, SameSite = cookieOptions.SameSite, Expires = DateTime.UtcNow.AddMinutes(60) });
                Response.Cookies.Append("RefreshToken", newRefreshToken, new CookieOptions { Path = cookieOptions.Path, HttpOnly = cookieOptions.HttpOnly, Secure = cookieOptions.Secure, SameSite = cookieOptions.SameSite, Expires = DateTime.UtcNow.AddDays(7) });
                return Ok(new { User = userResponse });
            }
            catch (ArgumentNullException ex) { return BadRequest(ex.Message); }
            catch (UnauthorizedAccessException ex) { return Unauthorized(ex.Message); }
            catch (Exception ex) { return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred while refreshing token.");}
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            var refreshTokenFromCookie = Request.Cookies["RefreshToken"];
            if (!string.IsNullOrEmpty(refreshTokenFromCookie))
            {
                try { await _authService.LogoutAsync(refreshTokenFromCookie); }
                catch (Exception) { /* Log error but proceed */ }
            }
            var cookieOptionsNone = new CookieOptions { HttpOnly = true, Secure = true, SameSite = SameSiteMode.None, Path = "/" };
            var cookieOptionsStrict = new CookieOptions { HttpOnly = true, Secure = true, SameSite = SameSiteMode.Strict, Path = "/" };
            Response.Cookies.Delete("AccessToken", cookieOptionsNone);
            Response.Cookies.Delete("RefreshToken", cookieOptionsNone);
            Response.Cookies.Delete("AccessToken", cookieOptionsStrict);
            Response.Cookies.Delete("RefreshToken", cookieOptionsStrict);

            return Ok("Logged out successfully.");
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(
            [FromBody] RegisterRequest request,
            [FromServices] IValidator<RegisterRequest> validator)
        {
            var validationResult = await validator.ValidateAsync(request);
            if (!validationResult.IsValid)
                return BadRequest(validationResult.ToDictionary());

            try
            {
                var token = await _authService.RegisterAsync(request);
                return Ok($"Registration successful. Please check your email to confirm your account.\nTOKEN (temporary): {token}");
            }
            catch (ArgumentException ex) when (ex.Message == "This email is already in use.")
            {
                return Conflict(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Registration failed.", errors = ex.Message });
            }
        }

        [HttpPost("verify-email")]
        public async Task<IActionResult> VerifyEmail(
            [FromBody] VerifyEmailRequest request, 
            [FromServices] IValidator<VerifyEmailRequest> validator)
        {
            var validationResult = await validator.ValidateAsync(request);
            if (!validationResult.IsValid)
                return BadRequest(validationResult.ToDictionary());

            try
            {
                await _authService.VerifyEmailAsync(request);
                // If VerifyEmailAsync throws on failure, this is only reached on success.
                return Ok("Email confirmed.");
            }
            catch (ArgumentException ex) when (ex.Message == "User not found.")
            {
                // This specific catch for "User not found" ensures a 404 Not Found.
                return NotFound(ex.Message);
            }
            catch (Exception ex) // Catches other errors, like "Email confirmation failed: <Identity Errors>"
            {
                // Log ex for server-side details
                // Return a BadRequest with the failure reasons.
                return BadRequest(new { message = "Email verification failed.", errors = ex.Message });
            }
        }
    }
}
