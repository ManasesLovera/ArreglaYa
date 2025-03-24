using Application.DTOs.Common;
using Application.DTOs.User;
using Application.Interfaces.Repository;
using AutoMapper;
using Domain;
using Domain.Models;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    /// <summary>
    /// Controller for managing user-related operations.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
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
        [HttpGet("id")]
        public async Task<ActionResult<UserResponse>> GetByIdAsync(string id)
        {
            var company = await _userRepo.GetByIdAsync(id);
            if (company == null)
            {
                return NotFound();
            }
            return Ok(_mapper.Map<UserResponse>(company));
        }

        /// <summary>
        /// Creates a new user.
        /// </summary>
        /// <param name="userRequest">The request data for creating a company.</param>
        [HttpPost]
        public async Task<ActionResult<UserResult>> CreateCompany(IValidator<CreateUserRequest> validator, [FromBody] CreateUserRequest userRequest)
        {
            try
            {
                var validationResult = await validator.ValidateAsync(userRequest);
                if (!validationResult.IsValid)
                {
                    return BadRequest(new UserResult(
                            false, null, string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage))
                        ));
                }

                var existingUser = await _userManager.FindByEmailAsync(userRequest.Email);
                if (existingUser != null)
                {
                    return Conflict(new UserResult(
                            false, null, "Email is already in use."
                        ));
                }

                var user = _mapper.Map<ApplicationUser>(userRequest);
                var result = await _userManager.CreateAsync(user, userRequest.Password);
                if (!result.Succeeded)
                {
                    return BadRequest(new UserResult(
                        false,
                        null,
                        string.Join("; ", result.Errors.Select(e => e.Description))
                    ));
                }

                var userResponse = _mapper.Map<UserResponse>(user);
                return CreatedAtAction(nameof(GetByIdAsync), new { id = user.Id }, 
                    new UserResult(true, userResponse, "User created successfully."));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new UserResult(false, null, ex.Message));
            }
        }

        /// <summary>
        /// Deletes a user by its ID.
        /// </summary>
        /// <param name="id">The ID of the user to delete.</param>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(string id)
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
        /// Updates the password of a user.
        /// </summary>
        /// <param name="id">The ID of the user.</param>
        /// <param name="request">The request containing the old and new passwords.</param>
        [HttpPut("{id}/password")]
        public async Task<IActionResult> UpdatePassword([FromRoute] string id, [FromBody] UpdatePasswordRequest request)
        {
            var user = await _userRepo.GetByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            var result = await _userManager.ChangePasswordAsync(user, request.OldPassword, request.NewPassword);
            if (!result.Succeeded)
            {
                return BadRequest(string.Join("; ", result.Errors.Select(e => e.Description)));
            }

            return NoContent();
        }
    }
}
