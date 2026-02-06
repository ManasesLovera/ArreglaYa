namespace Application.DTOs.Common
{
    /// <summary>
    /// Represents a request to update a user's password.
    /// </summary>
    /// <param name="CurrentPassword">The user's current password for verification.</param>
    /// <param name="NewPassword">The new password to set for the user.</param>
    public record UpdatePasswordRequest
    (
        string CurrentPassword,
        string NewPassword
    );
}
