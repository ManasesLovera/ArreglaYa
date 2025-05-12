using Application.DTOs.Common;
using Application.DTOs.User;
using Application.Interfaces.Repository;
using AutoMapper;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace WebAPI.Controllers
{
    /// <summary>
    /// Controller for managing user-related operations.
    /// </summary>
    [Route("api/user")]
    [ApiController]
    [Authorize]
    public class UserController : BaseController
    {
        private readonly IUserRepository _userRepo;
        private readonly UserManager<ApplicationUser> _userManager;

        public UserController(IUserRepository userRepo, UserManager<ApplicationUser> userManager, IMapper mapper)
        : base(mapper)
        {
            _userRepo = userRepo;
            _userManager = userManager;
        }

        /// <summary>
        /// Retrieves paginated list of users.
        /// </summary>
        /// <returns>A IEnumerable with all users.</returns>
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<PaginatedResponse<UserResponse>> GetPaginatedAsync([FromQuery] PaginationQuery query)
        {
            var users = await _userRepo.GetPaginatedAsync(query.PageIndex, query.PageSize);
            var totalRecords = await _userRepo.GetTotalCountAsync();
            return new PaginatedResponse<UserResponse>(
                _mapper.Map<IEnumerable<UserResponse>>(users),
                totalRecords,
                query.PageIndex,
                query.PageSize
            );
        }

        /// <summary>
        /// Retrieves a user by its ID.
        /// </summary>
        /// <param name="id">The ID of the user.</param>
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<UserResult>> GetByIdAsync(string id)
        {
            var user = await _userRepo.GetByIdAsync(id);
            if (user == null)
            {
                return NotFound(new UserResult(false, "User was not found"));
            }
            return Ok(new UserResult(true, User: _mapper.Map<UserResponse>(user)));
        }

        /// <summary>
        /// Deletes a user by its ID.
        /// </summary>
        /// <param name="id">The ID of the user to delete.</param>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteAsync(string id)
        {
            var user = await _userRepo.GetByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            await _userRepo.DeleteAsync(user.Id);
            return NoContent();
        }

        /// <summary>
        /// Updates the password of the currently logged-in user.
        /// </summary>
        /// <param name="request">The request containing the old and new passwords.</param>
        [Authorize]
        [HttpPatch("update-password")]
        public async Task<IActionResult> UpdatePasswordAsync([FromBody] UpdatePasswordRequest request)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)
                      ?? User.FindFirstValue(JwtRegisteredClaimNames.Sub); // fallback if needed

            if (string.IsNullOrEmpty(userId))
                return Unauthorized("Invalid token or missing user ID.");

            var user = await _userRepo.GetByIdAsync(userId);
            if (user == null)
                return NotFound();

            var result = await _userManager.ChangePasswordAsync(user, request.CurrentPassword, request.NewPassword);
            if (!result.Succeeded)
                return BadRequest(string.Join("; ", result.Errors.Select(e => e.Description)));

            return Ok("Password updated successfully.");
        }
    }
}
