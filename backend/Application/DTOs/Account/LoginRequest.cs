namespace Application.DTOs.Account
{
    /// <summary>
    /// Represents a login request containing user credentials.
    /// </summary>
    public sealed class LoginRequest
    {
        /// <summary>
        /// Gets or initializes the user's email address which acts as a user name.
        /// </summary>
        public required string Email { get; init; }

        /// <summary>
        /// Gets or initializes the user's password.
        /// </summary>
        public required string Password { get; init; }
    }
}
