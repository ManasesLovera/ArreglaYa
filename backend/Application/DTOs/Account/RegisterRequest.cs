namespace Application.DTOs.Account
{
    /// <summary>
    /// Represents a user registration request.
    /// </summary>
    public sealed class RegisterRequest
    {
        /// <summary>
        /// Gets or sets the full name of the user.
        /// </summary>
        public string? FullName { get; set; }

        /// <summary>
        /// Gets or sets the username for the account.
        /// </summary>
        public string? UserName { get; set; }

        /// <summary>
        /// Gets or sets the email address of the user.
        /// </summary>
        public string? Email { get; set; }

        /// <summary>
        /// Gets or sets the phone number of the user.
        /// </summary>
        public string? PhoneNumber { get; set; }

        /// <summary>
        /// Gets or sets the password for the account.
        /// </summary>
        public string? Password { get; set; }

        /// <summary>
        /// Gets or sets the password confirmation for validation.
        /// </summary>
        public string? PasswordConfirmation { get; set; }
    }
}
