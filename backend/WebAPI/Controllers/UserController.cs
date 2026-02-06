using Application.DTOs.Common;
using Application.DTOs.User;
using Application.Interfaces.Services;
using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;

namespace WebAPI.Controllers
{
    /// <summary>
    /// Controller for managing user-related operations.
    /// Provides endpoints for CRUD operations on users and password management.
    /// </summary>
    [Route("api/user")]
    [ApiController]
    [Authorize]
    public class UserController : BaseController
    {
        private readonly IUserService _userService;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserController"/> class.
        /// </summary>
        /// <param name="userService">The user service for user operations.</param>
        /// <param name="mapper">The AutoMapper instance.</param>
        public UserController(IUserService userService, IMapper mapper)
        : base(mapper)
        {
            _userService = userService;
        }

        /// <summary>
        /// Retrieves paginated list of users including roles.
        /// </summary>
        /// <param name="query">The pagination query parameters.</param>
        /// <returns>A paginated response of UserResponse objects.</returns>
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<PaginatedResponse<UserResponse>>> GetPaginatedAsync([FromQuery] PaginationQuery query)
        {
            var result = await _userService.GetPaginatedUsersAsync(query.PageIndex, query.PageSize);
            return Ok(result);
        }

        /// <summary>
        /// Retrieves a user by its ID including roles.
        /// </summary>
        /// <param name="id">The ID of the user.</param>
        /// <returns>The user result containing user information.</returns>
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<UserResult>> GetByIdAsync(string id)
        {
            var result = await _userService.GetUserByIdAsync(id);
            
            if (!result.IsSuccess)
            {
                return NotFound(result);
            }

            return Ok(result);
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
        public async Task<IActionResult> CreateUserAsync(
            [FromBody] CreateUserRequest request, 
            [FromServices] IValidator<CreateUserRequest> validator)
        {
            var validationResult = await validator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.ToDictionary());
            }

            var result = await _userService.CreateUserAsync(request);

            if (!result.IsSuccess)
            {
                // Check if it's a conflict (username/email already exists)
                if (result.Message.Contains("already taken") || result.Message.Contains("already registered"))
                {
                    return Conflict(result);
                }
                return BadRequest(result);
            }

            return CreatedAtAction(
                nameof(GetByIdAsync), 
                new { id = result.User.Id }, 
                result);
        }

        /// <summary>
        /// Deletes a user by its ID.
        /// </summary>
        /// <param name="id">The ID of the user to delete.</param>
        /// <returns>A no content result if successful, otherwise not found.</returns>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteAsync(string id)
        {
            var deleted = await _userService.DeleteUserAsync(id);
            if (!deleted)
            {
                return NotFound(new UserResult(false, "User not found."));
            }

            return NoContent();
        }

        /// <summary>
        /// Updates the password of the currently logged-in user.
        /// </summary>
        /// <param name="request">The request containing the old and new passwords.</param>
        /// <returns>An action result indicating success or failure.</returns>
        [Authorize]
        [HttpPatch("update-password")]
        public async Task<IActionResult> UpdatePasswordAsync([FromBody] UpdatePasswordRequest request)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)
                      ?? User.FindFirstValue(JwtRegisteredClaimNames.Sub);

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(ApiResponse<object>.Failure("Invalid token or missing user ID."));
            }

            var (success, message) = await _userService.UpdatePasswordAsync(
                userId, 
                request.CurrentPassword, 
                request.NewPassword);

            if (!success)
            {
                return BadRequest(ApiResponse<object>.Failure(message));
            }

            return Ok(ApiResponse<object>.Success(null, message));
        }
    }
}
