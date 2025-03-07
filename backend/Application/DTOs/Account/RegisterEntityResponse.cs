namespace Application.DTOs.Account
{
    public record RegisterEntityResponse
    (
        string? Id,
        string? Email,
        string? FullName,
        string? Username
    );
}
