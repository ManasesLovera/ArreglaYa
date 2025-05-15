using Application.DTOs.Account;
using Application.DTOs.User;
using AutoMapper;
using Domain.Models;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using WebAPI.Auth.Jwt;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : BaseController
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly JwtTokenGenerator _tokenGenerator;

        public AuthController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, JwtTokenGenerator jwtTokenGenerator,IMapper mapper) : base(mapper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenGenerator = jwtTokenGenerator;
        }

        [Authorize]
        [HttpGet("me")]
        public async Task<IActionResult> Me()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
                return Unauthorized();
            
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) 
                return NotFound();

            var userResponse = _mapper.Map<UserResponse>(user);
            userResponse.Roles = (await _userManager.GetRolesAsync(user)).ToArray();
            return Ok(userResponse);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(
            [FromBody] LoginRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user == null)
                return Unauthorized("Invalid email or password.");

            var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, lockoutOnFailure: true);
            if (!result.Succeeded)
                return Unauthorized("Invalid email or password.");

            if (!user.EmailConfirmed)
                return Unauthorized("Email not confirmed.");

            var roles = await _userManager.GetRolesAsync(user);
            var accessToken = _tokenGenerator.Generate(user, roles);
            var refreshToken = Guid.NewGuid().ToString(); // TODO: Replace with secure token generation

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);

            // Set HttpOnly cookies
            Response.Cookies.Append("AccessToken", accessToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = DateTime.UtcNow.AddMinutes(60)
            });

            Response.Cookies.Append("RefreshToken", refreshToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = DateTime.UtcNow.AddDays(7)
            });

            var userResponse = _mapper.Map<UserResponse>(user);
            userResponse.Roles = (await _userManager.GetRolesAsync(user)).ToArray();
            var response = new
            {
                User = userResponse
            };

            return Ok(response); // Or return 204 NoContent
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken()
        {
            var refreshToken = Request.Cookies["RefreshToken"];

            if (string.IsNullOrEmpty(refreshToken))
                return Unauthorized("Refresh token is missing.");

            var user = await _userManager.Users
                .FirstOrDefaultAsync(u => u.RefreshToken == refreshToken);

            if (user == null || user.RefreshTokenExpiryTime < DateTime.UtcNow)
                return Unauthorized("Invalid or expired refresh token.");

            var roles = await _userManager.GetRolesAsync(user);
            var newAccessToken = _tokenGenerator.Generate(user, roles);

            // Optional: rotate refresh token for better security
            var newRefreshToken = Guid.NewGuid().ToString();
            user.RefreshToken = newRefreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
            await _userManager.UpdateAsync(user);

            // Set new tokens as HttpOnly cookies
            Response.Cookies.Append("AccessToken", newAccessToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddMinutes(60)
            });

            Response.Cookies.Append("RefreshToken", newRefreshToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddDays(7)
            });

            return Ok(_mapper.Map<UserResponse>(user));
        }


        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            var refreshToken = Request.Cookies["RefreshToken"];

            if (!string.IsNullOrEmpty(refreshToken))
            {
                var user = await _userManager.Users
                    .FirstOrDefaultAsync(u => u.RefreshToken == refreshToken);

                if (user != null)
                {
                    user.RefreshToken = null;
                    user.RefreshTokenExpiryTime = DateTime.MinValue;
                    await _userManager.UpdateAsync(user);
                }
            }

            // Clear cookies from browser
            Response.Cookies.Delete("AccessToken");
            Response.Cookies.Delete("RefreshToken");

            return Ok("Logged out successfully.");
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(
            [FromBody] RegisterRequest request,
            [FromServices] IValidator<RegisterRequest> validator)
        {
            var validation = await validator.ValidateAsync(request);
            if (!validation.IsValid)
                return BadRequest(validation.ToDictionary());

            var userExist = await _userManager.FindByEmailAsync(request.Email!);

            if (userExist == null)
                return Conflict("This email is in used");
            

            var user = _mapper.Map<ApplicationUser>(request);

            var result = await _userManager.CreateAsync(user, request.Password!);
            if (!result.Succeeded)
            {
                return Conflict(result.Errors);
            }

            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

            // TODO: send confirmation email with the token + user.Id
            // e.g., /api/auth/verify-email?userId=...&token=...

            // TODO: When sending confirmation email works, stop sending the token in the response to the client.
            return Ok($"Registration successful. Please check your email to confirm your account.\nTOKEN (temporary): {token}");
        }

        [HttpPost("verify-email")]
        public async Task<IActionResult> VerifyEmail(
            [FromBody] VerifyEmailRequest request, 
            [FromServices] IValidator<VerifyEmailRequest> validator)
        {
            var validation = await validator.ValidateAsync(request);
            if (!validation.IsValid)
                return BadRequest(validation.ToDictionary());

            var user = await _userManager.FindByIdAsync(request.UserId);
            if (user == null)
                return NotFound("User not found");

            var result = await _userManager.ConfirmEmailAsync(user, request.Token);
            return result.Succeeded ? Ok("Email confirmed.") : BadRequest(result.Errors);
        }
    }
}
