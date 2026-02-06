using Application.DTOs.Common;
using Application.DTOs.User;
using System.Threading.Tasks;

namespace Application.Interfaces.Services
{
    /// <summary>
    /// Service interface for user management operations.
    /// Provides abstraction over Identity UserManager operations.
    /// </summary>
    public interface IUserService
    {
        /// <summary>
        /// Retrieves a paginated list of users with their roles.
        /// </summary>
        /// <param name="pageIndex">The page index (starting from 1).</param>
        /// <param name="pageSize">The number of items per page.</param>
        /// <returns>A paginated response containing user information.</returns>
        Task<PaginatedResponse<UserResponse>> GetPaginatedUsersAsync(int pageIndex, int pageSize);

        /// <summary>
        /// Retrieves a user by their unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the user.</param>
        /// <returns>The user result containing user information if found, otherwise null.</returns>
        Task<UserResult> GetUserByIdAsync(string id);

        /// <summary>
        /// Creates a new user with the specified role.
        /// </summary>
        /// <param name="request">The user creation request containing user details.</param>
        /// <param name="role">The role to assign to the new user (default: "Client").</param>
        /// <returns>The result of the user creation operation.</returns>
        Task<UserResult> CreateUserAsync(CreateUserRequest request, string role = "Client");

        /// <summary>
        /// Deletes a user by their unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the user to delete.</param>
        /// <returns>True if the user was successfully deleted, otherwise false.</returns>
        Task<bool> DeleteUserAsync(string id);

        /// <summary>
        /// Updates the password for a specific user.
        /// </summary>
        /// <param name="userId">The unique identifier of the user.</param>
        /// <param name="currentPassword">The user's current password.</param>
        /// <param name="newPassword">The new password to set.</param>
        /// <returns>True if the password was successfully updated, otherwise false with error messages.</returns>
        Task<(bool Success, string Message)> UpdatePasswordAsync(string userId, string currentPassword, string newPassword);
    }
}
