namespace Application.DTOs.User
{
    /// <summary>
    /// Represents the result of a user operation.
    /// </summary>
    /// <param name="Success">Indicates whether the operation was successful.</param>
    /// <param name="Message">Optional message describing the result.</param>
    /// <param name="User">Optional user response data if the operation returns a user.</param>
    public record UserResult
    (
        bool Success,
        string? Message = null,
        UserResponse? User = null
    );
}
