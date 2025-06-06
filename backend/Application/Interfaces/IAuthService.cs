using Application.DTOs.Account; // For LoginRequest, RegisterRequest, VerifyEmailRequest
using Application.DTOs.User;    // For UserResponse
using System.Threading.Tasks;

namespace Application.Interfaces
{
    // Generic result DTO for service layer responses
    public class ServiceResponse<T>
    {
        public bool Success { get; set; }
        public T Data { get; set; }
        public string ErrorMessage { get; set; }
        public string[] Errors { get; set; } // For multiple validation errors

        // Static factory methods for convenience
        public static ServiceResponse<T> Succeed(T data) => new ServiceResponse<T> { Success = true, Data = data };
        public static ServiceResponse<T> Fail(string errorMessage, string[] errors = null) => new ServiceResponse<T> { Success = false, ErrorMessage = errorMessage, Errors = errors };
    }

    // Specific response for login which might include tokens not directly part of UserResponse
    public class LoginServiceResponse
    {
        public bool Success { get; set; }
        public UserResponse User { get; set; }
        public string AccessToken { get; set; } // Will be set as HttpOnly cookie by controller
        public string RefreshToken { get; set; } // Will be set as HttpOnly cookie by controller
        public string ErrorMessage { get; set; }
        public string[] Errors { get; set; }

        public static LoginServiceResponse Succeed(UserResponse user, string accessToken, string refreshToken) =>
            new LoginServiceResponse { Success = true, User = user, AccessToken = accessToken, RefreshToken = refreshToken };
        public static LoginServiceResponse Fail(string errorMessage, string[] errors = null) =>
            new LoginServiceResponse { Success = false, ErrorMessage = errorMessage, Errors = errors };
    }

    // Specific response for registration
    public class RegisterServiceResponse
    {
        public bool Success { get; set; }
        public UserResponse User {get; set; } // Optional: return user on successful registration
        public string ConfirmationToken { get; set; } // For email verification
        public string ErrorMessage { get; set; }
        public string[] Errors { get; set; }

        public static RegisterServiceResponse Succeed(UserResponse user, string confirmationToken = null) =>
            new RegisterServiceResponse { Success = true, User = user, ConfirmationToken = confirmationToken };
        public static RegisterServiceResponse Fail(string errorMessage, string[] errors = null) =>
            new RegisterServiceResponse { Success = false, ErrorMessage = errorMessage, Errors = errors };
    }

    public interface IAuthService
    {
        Task<ServiceResponse<UserResponse>> GetCurrentUserAsync(string userId);
        Task<LoginServiceResponse> LoginAsync(LoginRequest request);
        Task<LoginServiceResponse> RefreshTokenAsync(string currentRefreshToken); // Controller handles reading cookie
        Task<ServiceResponse<string>> LogoutAsync(string currentRefreshToken); // Controller handles reading cookie & clearing
        Task<RegisterServiceResponse> RegisterAsync(RegisterRequest request);
        Task<ServiceResponse<string>> VerifyEmailAsync(VerifyEmailRequest request);
    }
}
