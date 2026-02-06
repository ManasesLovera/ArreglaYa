namespace Application.Auth
{
    /// <summary>
    /// Configuration settings for JWT token generation.
    /// </summary>
    public class JwtSettings
    {
        /// <summary>
        /// Gets or sets the secret key used for signing tokens.
        /// </summary>
        public string? Secret { get; set; }

        /// <summary>
        /// Gets or sets the issuer of the tokens.
        /// </summary>
        public string? Issuer { get; set; }

        /// <summary>
        /// Gets or sets the audience for the tokens.
        /// </summary>
        public string? Audience { get; set; }

        /// <summary>
        /// Gets or sets the token expiry time in minutes.
        /// Default is 60 minutes.
        /// </summary>
        public int ExpiryMinutes { get; set; } = 60;
    }
}
