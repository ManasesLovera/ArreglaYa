using Application.DTOs.Account;
using Application.DTOs.User;
using Microsoft.AspNetCore.Mvc; // Required for IActionResult (though not directly used in interface method signatures here)
using System.Security.Claims; // Required for ClaimsPrincipal
using System.Threading.Tasks; // Required for Task

namespace Application.Interfaces.Services
{
    /// <summary>
    /// Defines the contract for authentication services.
    /// </summary>
    public interface IAuthService
    {
        /// <summary>
        /// Retrieves the current authenticated user's details.
        /// </summary>
        /// <param name="userPrincipal">The ClaimsPrincipal representing the authenticated user.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the <see cref="UserResponse"/> DTO for the current user, or null if the user cannot be found or is not authenticated.</returns>
        Task<UserResponse> GetCurrentUserAsync(ClaimsPrincipal userPrincipal);

        /// <summary>
        /// Attempts to log in a user with the provided credentials.
        /// </summary>
        /// <param name="request">The <see cref="LoginRequest"/> DTO containing login credentials.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a tuple with the <see cref="UserResponse"/> DTO, an access token, and a refresh token upon successful login.</returns>
        /// <exception cref="System.UnauthorizedAccessException">Thrown if login fails due to invalid credentials, unconfirmed email, or other authentication issues.</exception>
        /// <exception cref="System.Exception">Thrown if there's an issue updating the user with the refresh token.</exception>
        Task<(UserResponse User, string AccessToken, string RefreshToken)> LoginAsync(LoginRequest request);

        /// <summary>
        /// Refreshes an access token using a valid refresh token.
        /// </summary>
        /// <param name="existingRefreshToken">The existing refresh token.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a tuple with a new access token, a new refresh token, and the <see cref="UserResponse"/> DTO.</returns>
        /// <exception cref="System.ArgumentNullException">Thrown if the <paramref name="existingRefreshToken"/> is null or empty.</exception>
        /// <exception cref="System.UnauthorizedAccessException">Thrown if the refresh token is invalid, expired, or the associated user is not found.</exception>
        /// <exception cref="System.Exception">Thrown if there's an issue updating the user with the new refresh token.</exception>
        Task<(string NewAccessToken, string NewRefreshToken, UserResponse User)> RefreshTokenAsync(string? existingRefreshToken);

        /// <summary>
        /// Logs out a user by invalidating their refresh token on the server side.
        /// </summary>
        /// <param name="existingRefreshToken">The refresh token of the user to log out. Can be null if not available.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task LogoutAsync(string? existingRefreshToken);

        /// <summary>
        /// Registers a new user.
        /// </summary>
        /// <param name="request">The <see cref="RegisterRequest"/> DTO containing registration details.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the email confirmation token for the newly registered user.</returns>
        /// <exception cref="System.ArgumentException">Thrown if the email provided in <paramref name="request"/> is already in use.</exception>
        /// <exception cref="System.Exception">Thrown if user creation fails due to Identity errors or other issues.</exception>
        Task<string> RegisterAsync(RegisterRequest request);

        /// <summary>
        /// Verifies a user's email address using a user ID and a confirmation token.
        /// </summary>
        /// <param name="request">The <see cref="VerifyEmailRequest"/> DTO containing the user ID and token.</param>
        /// <returns>A task that represents the asynchronous operation. The task result is true if email verification is successful.</returns>
        /// <exception cref="System.ArgumentException">Thrown if the user specified by <see cref="VerifyEmailRequest.UserId"/> is not found.</exception>
        /// <exception cref="System.Exception">Thrown if email confirmation fails due to an invalid token or other Identity errors.</exception>
        Task<bool> VerifyEmailAsync(VerifyEmailRequest request);
    }
}
