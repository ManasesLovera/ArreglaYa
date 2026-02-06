using Application.DTOs.Common;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;
using Application.Interfaces.Services;

namespace WebAPI.Controllers.Base
{
    /// <summary>
    /// Base controller for user-type entities (Company, Client, Admin).
    /// Extends CustomBaseController with additional user-specific operations like UpdatePassword.
    /// </summary>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    /// <typeparam name="TCreateRequest">The DTO for creating entities.</typeparam>
    /// <typeparam name="TUpdateRequest">The DTO for updating entities.</typeparam>
    /// <typeparam name="TResponse">The response DTO type.</typeparam>
    public abstract class UserBaseController<TEntity, TCreateRequest, TUpdateRequest, TResponse> 
        : CustomBaseController<TEntity, TCreateRequest, TUpdateRequest, TResponse>
        where TEntity : class
        where TCreateRequest : class
        where TUpdateRequest : class
        where TResponse : class
    {
        /// <summary>
        /// User service for user-specific operations.
        /// </summary>
        protected readonly IUserService _userService;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserBaseController{TEntity, TCreateRequest, TUpdateRequest, TResponse}"/> class.
        /// </summary>
        /// <param name="mapper">The AutoMapper instance.</param>
        /// <param name="logger">The logger instance.</param>
        /// <param name="userService">The user service for user operations.</param>
        protected UserBaseController(
            IMapper mapper, 
            ILogger logger,
            IUserService userService) 
            : base(mapper, logger)
        {
            _userService = userService;
        }

        /// <summary>
        /// Updates the password of the currently authenticated user.
        /// This endpoint is available to all authenticated users to change their own password.
        /// </summary>
        /// <param name="request">The password update request containing current and new passwords.</param>
        /// <returns>An action result indicating the success or failure of the password update.</returns>
        [Authorize]
        [HttpPatch("update-password")]
        public virtual async Task<IActionResult> UpdatePasswordAsync([FromBody] UpdatePasswordRequest request)
        {
            // Extract user ID from claims
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)
                      ?? User.FindFirstValue(JwtRegisteredClaimNames.Sub);

            if (string.IsNullOrEmpty(userId))
            {
                LogError("Unauthorized password update attempt - missing user ID in token.");
                return Unauthorized(FailureResponse<object>("Invalid token or missing user ID."));
            }

            LogInformation($"User {userId} attempting to update password.");

            var (success, message) = await _userService.UpdatePasswordAsync(
                userId, 
                request.CurrentPassword, 
                request.NewPassword);

            if (!success)
            {
                LogError($"Password update failed for user {userId}: {message}");
                return BadRequest(FailureResponse<object>(message));
            }

            LogInformation($"Password updated successfully for user {userId}.");
            return Ok(SuccessResponse<object>(null, message));
        }

        /// <summary>
        /// Gets the currently authenticated user's ID from the claims.
        /// </summary>
        /// <returns>The user ID if found, otherwise null.</returns>
        protected string GetCurrentUserId()
        {
            return User.FindFirstValue(ClaimTypes.NameIdentifier)
                ?? User.FindFirstValue(JwtRegisteredClaimNames.Sub);
        }

        /// <summary>
        /// Checks if the current user has the specified role.
        /// </summary>
        /// <param name="role">The role name to check.</param>
        /// <returns>True if the user has the role, otherwise false.</returns>
        protected bool HasRole(string role)
        {
            return User.IsInRole(role);
        }
    }
}
