using Application.DTOs.Common;
using Application.DTOs.User;
using Application.Interfaces.Repository;
using AutoMapper;
using Domain.Models;
using FluentValidation;
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
        /// Retrieves paginated list of users including roles.
        /// </summary>
        /// <returns>A paginated response of UserResponse objects.</returns>
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<PaginatedResponse<UserResponse>> GetPaginatedAsync([FromQuery] PaginationQuery query)
        {
            var users = await _userRepo.GetPaginatedAsync(query.PageIndex, query.PageSize);
            var totalRecords = await _userRepo.GetTotalCountAsync();

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
                query.PageIndex,
                query.PageSize
            );
        }

        /// <summary>
        /// Retrieves a user by its ID including roles.
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
            var roles = await _userManager.GetRolesAsync(user);
            var userResponse = _mapper.Map<UserResponse>(user);
            userResponse.Roles = (await _userManager.GetRolesAsync(user)).ToArray();
            return Ok(new UserResult(true, User: userResponse));
        }

        /// <summary>
        /// Creates a new user with the role of "Client". Only accessible by Admins.
        /// </summary>
        /// <param name="request">The request containing user registration data.</param>
        /// <param name="validator">The validator used to validate the incoming request model.</param>
        /// <returns>
        /// Returns a <see cref="CreatedAtActionResult"/> if the user is successfully created,
        /// a <see cref="BadRequestObjectResult"/> if the request is invalid or creation fails,
        /// or a <see cref="ConflictObjectResult"/> if the username or email already exists.
        /// </returns>
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateUserAsync([FromBody] CreateUserRequest request, IValidator<CreateUserRequest> validator)
        {
            var validationResult = validator.Validate(request);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.ToDictionary());
            }

            // Check if username or email already exists
            var existingUserByName = await _userManager.FindByNameAsync(request.Username);
            if (existingUserByName != null)
                return Conflict($"Username '{request.Username}' is already taken.");

            var existingUserByEmail = await _userManager.FindByEmailAsync(request.Email);
            if (existingUserByEmail != null)
                return Conflict($"Email '{request.Email}' is already registered.");

            var user = _mapper.Map<ApplicationUser>(existingUserByEmail);

            var result = await _userManager.CreateAsync(user, request.Password);
            if (!result.Succeeded)
            {
                var errors = string.Join("; ", result.Errors.Select(e => e.Description));
                return BadRequest(errors);
            }

            await _userManager.AddToRoleAsync(user, "Client");
            return CreatedAtAction(nameof(GetByIdAsync), new { id = user.Id }, new { Message = "User created successfully", UserId = user.Id });
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
