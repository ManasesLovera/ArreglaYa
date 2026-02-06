namespace Application.DTOs.Account
{
    /// <summary>
    /// Represents a request to verify a user's email address.
    /// </summary>
    /// <param name="UserId">The unique identifier of the user.</param>
    /// <param name="Token">The email verification token.</param>
    public sealed record VerifyEmailRequest
    (
        string UserId,
        string Token
    );
}
