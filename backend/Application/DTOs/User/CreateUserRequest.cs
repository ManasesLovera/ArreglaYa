namespace Application.DTOs.User
{
    /// <summary>
    /// Represents a request to create a new user.
    /// </summary>
    /// <param name="Username">The unique username for the user.</param>
    /// <param name="Fullname">The full name of the user.</param>
    /// <param name="Email">The email address of the user.</param>
    /// <param name="Password">The password for the user account.</param>
    public record CreateUserRequest
    (
        string Username,
        string Fullname,
        string Email,
        string Password
    );
}
