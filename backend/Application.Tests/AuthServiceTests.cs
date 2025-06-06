using Xunit;
using Moq;
using Application.Services;
using Application.Interfaces;
using Application.DTOs.Account;
using Application.DTOs.User;
using Domain.Models; // For ApplicationUser
using Microsoft.AspNetCore.Identity;
using AutoMapper;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using WebAPI.Auth.Jwt; // For JwtTokenGenerator
using System; // For DateTime

namespace Application.Tests
{
    public class AuthServiceTests
    {
        private readonly Mock<UserManager<ApplicationUser>> _mockUserManager;
        private readonly Mock<SignInManager<ApplicationUser>> _mockSignInManager;
        private readonly Mock<JwtTokenGenerator> _mockTokenGenerator; // Assuming concrete class or its interface if available
        private readonly Mock<IMapper> _mockMapper;
        private readonly AuthService _authService;

        // Helper to create UserManager mock
        public static Mock<UserManager<TUser>> MockUserManager<TUser>() where TUser : class
        {
            var store = new Mock<IUserStore<TUser>>();
            // Add other IUser interfaces as needed by methods being tested e.g. IUserPasswordStore etc.
            var userManager = new Mock<UserManager<TUser>>(store.Object, null, null, null, null, null, null, null, null);
            userManager.Object.UserValidators.Add(new UserValidator<TUser>());
            userManager.Object.PasswordValidators.Add(new PasswordValidator<TUser>());
            return userManager;
        }

        // Helper to create SignInManager mock
        public static Mock<SignInManager<TUser>> MockSignInManager<TUser>(UserManager<TUser> userManager) where TUser : class
        {
            var contextAccessor = new Mock<Microsoft.AspNetCore.Http.IHttpContextAccessor>();
            var claimsFactory = new Mock<IUserClaimsPrincipalFactory<TUser>>();
            return new Mock<SignInManager<TUser>>(userManager, contextAccessor.Object, claimsFactory.Object, null, null, null, null);
        }


        public AuthServiceTests()
        {
            _mockUserManager = MockUserManager<ApplicationUser>();
            _mockSignInManager = MockSignInManager<ApplicationUser>(_mockUserManager.Object);
            _mockTokenGenerator = new Mock<JwtTokenGenerator>(); // If JwtTokenGenerator has dependencies, they also need mocking or a concrete instance.
            _mockMapper = new Mock<IMapper>();

            _authService = new AuthService(
                _mockUserManager.Object,
                _mockSignInManager.Object,
                _mockTokenGenerator.Object,
                _mockMapper.Object
            );

            // Default Mapper Setup
            _mockMapper.Setup(m => m.Map<UserResponse>(It.IsAny<ApplicationUser>()))
                .Returns((ApplicationUser src) => src == null ? null : new UserResponse { Id = src.Id, Email = src.Email, UserName = src.UserName, FullName = src.FullName });
            _mockMapper.Setup(m => m.Map<ApplicationUser>(It.IsAny<RegisterRequest>()))
                .Returns((RegisterRequest src) => src == null ? null : new ApplicationUser { Email = src.Email, UserName = src.UserName ?? src.Email, FullName = src.FullName });

            // Default Token Generator Setup
            _mockTokenGenerator.Setup(t => t.Generate(It.IsAny<ApplicationUser>(), It.IsAny<IList<string>>()))
                .Returns("mock_access_token");
        }

        // --- GetCurrentUserAsync Tests ---
        [Fact]
        public async Task GetCurrentUserAsync_UserFound_ReturnsSuccessWithUserData()
        {
            var userId = "test-user-id";
            var appUser = new ApplicationUser { Id = userId, Email = "test@example.com", UserName = "testuser" };
            _mockUserManager.Setup(um => um.FindByIdAsync(userId)).ReturnsAsync(appUser);
            _mockUserManager.Setup(um => um.GetRolesAsync(appUser)).ReturnsAsync(new List<string> { "User" });

            var result = await _authService.GetCurrentUserAsync(userId);

            Assert.True(result.Success);
            Assert.NotNull(result.Data);
            Assert.Equal(userId, result.Data.Id);
            Assert.Contains("User", result.Data.Roles);
        }

        [Fact]
        public async Task GetCurrentUserAsync_UserNotFound_ReturnsFail()
        {
            _mockUserManager.Setup(um => um.FindByIdAsync(It.IsAny<string>())).ReturnsAsync((ApplicationUser)null);
            var result = await _authService.GetCurrentUserAsync("nonexistent-id");
            Assert.False(result.Success);
            Assert.Equal("User not found.", result.ErrorMessage);
        }

        [Fact]
        public async Task GetCurrentUserAsync_NullOrEmptyUserId_ReturnsFail()
        {
            var result = await _authService.GetCurrentUserAsync(null);
            Assert.False(result.Success);
            Assert.Equal("User ID cannot be null or empty.", result.ErrorMessage);
        }

        // --- LoginAsync Tests ---
        [Fact]
        public async Task LoginAsync_ValidCredentialsAndConfirmedEmail_ReturnsSuccessWithTokensAndUser()
        {
            var request = new LoginRequest { Email = "test@example.com", Password = "Password123!" };
            var appUser = new ApplicationUser { Id = "1", Email = request.Email, EmailConfirmed = true, UserName = "testuser" };
            _mockUserManager.Setup(um => um.FindByEmailAsync(request.Email)).ReturnsAsync(appUser);
            _mockSignInManager.Setup(sm => sm.CheckPasswordSignInAsync(appUser, request.Password, true)).ReturnsAsync(SignInResult.Success);
            _mockUserManager.Setup(um => um.GetRolesAsync(appUser)).ReturnsAsync(new List<string>());
            _mockUserManager.Setup(um => um.UpdateAsync(It.IsAny<ApplicationUser>())).ReturnsAsync(IdentityResult.Success);


            var result = await _authService.LoginAsync(request);

            Assert.True(result.Success);
            Assert.NotNull(result.User);
            Assert.Equal(appUser.Email, result.User.Email);
            Assert.Equal("mock_access_token", result.AccessToken);
            Assert.NotNull(result.RefreshToken);
        }

        [Fact]
        public async Task LoginAsync_UserNotFound_ReturnsFail()
        {
            var request = new LoginRequest { Email = "test@example.com", Password = "Password123!" };
            _mockUserManager.Setup(um => um.FindByEmailAsync(request.Email)).ReturnsAsync((ApplicationUser)null);

            var result = await _authService.LoginAsync(request);

            Assert.False(result.Success);
            Assert.Equal("Invalid email or password.", result.ErrorMessage);
        }

        [Fact]
        public async Task LoginAsync_EmailNotConfirmed_ReturnsFail()
        {
            var request = new LoginRequest { Email = "test@example.com", Password = "Password123!" };
            var appUser = new ApplicationUser { Email = request.Email, EmailConfirmed = false };
            _mockUserManager.Setup(um => um.FindByEmailAsync(request.Email)).ReturnsAsync(appUser);

            var result = await _authService.LoginAsync(request);

            Assert.False(result.Success);
            Assert.Equal("Email not confirmed. Please verify your email address.", result.ErrorMessage);
        }

        [Fact]
        public async Task LoginAsync_IncorrectPassword_ReturnsFail()
        {
            var request = new LoginRequest { Email = "test@example.com", Password = "WrongPassword" };
            var appUser = new ApplicationUser { Email = request.Email, EmailConfirmed = true };
            _mockUserManager.Setup(um => um.FindByEmailAsync(request.Email)).ReturnsAsync(appUser);
            _mockSignInManager.Setup(sm => sm.CheckPasswordSignInAsync(appUser, request.Password, true)).ReturnsAsync(SignInResult.Failed);

            var result = await _authService.LoginAsync(request);

            Assert.False(result.Success);
            Assert.Equal("Invalid email or password.", result.ErrorMessage);
        }

        [Fact]
        public async Task LoginAsync_FailedToUpdateUserSession_ReturnsFail()
        {
            var request = new LoginRequest { Email = "test@example.com", Password = "Password123!" };
            var appUser = new ApplicationUser { Id = "1", Email = request.Email, EmailConfirmed = true, UserName = "testuser" };
            _mockUserManager.Setup(um => um.FindByEmailAsync(request.Email)).ReturnsAsync(appUser);
            _mockSignInManager.Setup(sm => sm.CheckPasswordSignInAsync(appUser, request.Password, true)).ReturnsAsync(SignInResult.Success);
            _mockUserManager.Setup(um => um.GetRolesAsync(appUser)).ReturnsAsync(new List<string>());
            _mockUserManager.Setup(um => um.UpdateAsync(It.IsAny<ApplicationUser>())).ReturnsAsync(IdentityResult.Failed(new IdentityError { Description = "DB error" }));

            var result = await _authService.LoginAsync(request);

            Assert.False(result.Success);
            Assert.Equal("Failed to update user session. Please try again.", result.ErrorMessage);
            Assert.Contains("DB error", result.Errors);
        }


        // --- RegisterAsync Tests ---
        [Fact]
        public async Task RegisterAsync_NewUser_ReturnsSuccessWithTokenAndUser()
        {
            var request = new RegisterRequest { FullName="Test User", Email = "new@example.com", Password = "Password123!", UserName = "newuser" };
            _mockUserManager.Setup(um => um.FindByEmailAsync(request.Email)).ReturnsAsync((ApplicationUser)null);
            _mockUserManager.Setup(um => um.CreateAsync(It.IsAny<ApplicationUser>(), request.Password)).ReturnsAsync(IdentityResult.Success);
            _mockUserManager.Setup(um => um.GenerateEmailConfirmationTokenAsync(It.IsAny<ApplicationUser>())).ReturnsAsync("confirm_token");
            _mockUserManager.Setup(um => um.GetRolesAsync(It.IsAny<ApplicationUser>())).ReturnsAsync(new List<string>());


            var result = await _authService.RegisterAsync(request);

            Assert.True(result.Success);
            Assert.NotNull(result.User);
            Assert.Equal(request.Email, result.User.Email);
            Assert.Equal("confirm_token", result.ConfirmationToken);
        }

        [Fact]
        public async Task RegisterAsync_EmailAlreadyExists_ReturnsFail()
        {
            var request = new RegisterRequest { Email = "existing@example.com", Password = "Password123!" };
            var existingUser = new ApplicationUser { Email = request.Email };
            _mockUserManager.Setup(um => um.FindByEmailAsync(request.Email)).ReturnsAsync(existingUser);

            var result = await _authService.RegisterAsync(request);

            Assert.False(result.Success);
            Assert.Equal("This email is already in use.", result.ErrorMessage);
        }

        [Fact]
        public async Task RegisterAsync_UserCreationFails_ReturnsFail()
        {
            var request = new RegisterRequest { Email = "new@example.com", Password = "Password123!" };
             _mockUserManager.Setup(um => um.FindByEmailAsync(request.Email)).ReturnsAsync((ApplicationUser)null);
            _mockUserManager.Setup(um => um.CreateAsync(It.IsAny<ApplicationUser>(), request.Password))
                .ReturnsAsync(IdentityResult.Failed(new IdentityError { Description = "Create failed" }));

            var result = await _authService.RegisterAsync(request);

            Assert.False(result.Success);
            Assert.Equal("User creation failed.", result.ErrorMessage);
            Assert.Contains("Create failed", result.Errors);
        }

        // --- VerifyEmailAsync Tests ---
        [Fact]
        public async Task VerifyEmailAsync_ValidTokenAndUser_ReturnsSuccess()
        {
            var request = new VerifyEmailRequest("user-id", "valid-token");
            var appUser = new ApplicationUser { Id = request.UserId };
            _mockUserManager.Setup(um => um.FindByIdAsync(request.UserId)).ReturnsAsync(appUser);
            _mockUserManager.Setup(um => um.ConfirmEmailAsync(appUser, request.Token)).ReturnsAsync(IdentityResult.Success);

            var result = await _authService.VerifyEmailAsync(request);

            Assert.True(result.Success);
            Assert.Equal("Email confirmed successfully.", result.Data);
        }

        [Fact]
        public async Task VerifyEmailAsync_UserNotFound_ReturnsFail()
        {
            var request = new VerifyEmailRequest("user-id", "valid-token");
            _mockUserManager.Setup(um => um.FindByIdAsync(request.UserId)).ReturnsAsync((ApplicationUser)null);

            var result = await _authService.VerifyEmailAsync(request);

            Assert.False(result.Success);
            Assert.Equal("User not found.", result.ErrorMessage);
        }

        [Fact]
        public async Task VerifyEmailAsync_InvalidToken_ReturnsFail()
        {
            var request = new VerifyEmailRequest("user-id", "invalid-token");
            var appUser = new ApplicationUser { Id = request.UserId };
            _mockUserManager.Setup(um => um.FindByIdAsync(request.UserId)).ReturnsAsync(appUser);
            _mockUserManager.Setup(um => um.ConfirmEmailAsync(appUser, request.Token))
                .ReturnsAsync(IdentityResult.Failed(new IdentityError { Description = "Invalid token" }));

            var result = await _authService.VerifyEmailAsync(request);

            Assert.False(result.Success);
            Assert.Equal("Email confirmation failed.", result.ErrorMessage);
            Assert.Contains("Invalid token", result.Errors);
        }

        // --- RefreshTokenAsync Tests ---
        [Fact]
        public async Task RefreshTokenAsync_ValidRefreshToken_ReturnsSuccessWithNewTokensAndUser()
        {
            var refreshToken = "valid-refresh-token";
            var appUser = new ApplicationUser { Id = "1", Email = "test@example.com", RefreshToken = refreshToken, RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(1) };

            // Mocking Users.FirstOrDefaultAsync is tricky. For simplicity, we'll assume FindByIdAsync or similar is used if possible,
            // or this part of the logic might be harder to unit test directly without a concrete DbContext or more complex mocking.
            // The actual AuthService uses _userManager.Users.FirstOrDefaultAsync. This requires mocking IQueryable which is complex.
            // A common workaround is to introduce a method in a repository/service that gets the user by refresh token, and mock that.
            // For this test, we'll assume _mockUserManager can be set up to return the user based on some criteria.
            // Let's refine the AuthService to make it more testable or use a more advanced mocking setup for IQueryable.
            // For now, this test might be more conceptual or require specific UserManager setup.
            // Let's assume we add a method to UserManager or a repository: FindUserByRefreshTokenAsync
            _mockUserManager.Setup(um => um.Users).Returns(new List<ApplicationUser> { appUser }.AsQueryable()); // Basic mock for Users
             _mockUserManager.Setup(um => um.GetRolesAsync(appUser)).ReturnsAsync(new List<string>());
            _mockUserManager.Setup(um => um.UpdateAsync(It.IsAny<ApplicationUser>())).ReturnsAsync(IdentityResult.Success);
            _mockTokenGenerator.Setup(tg => tg.Generate(appUser, It.IsAny<IList<string>>())).Returns("new_access_token");


            var result = await _authService.RefreshTokenAsync(refreshToken);

            Assert.True(result.Success);
            Assert.NotNull(result.User);
            Assert.Equal("new_access_token", result.AccessToken);
            Assert.NotNull(result.RefreshToken); // This will be the new refresh token from Guid.NewGuid()
            Assert.NotEqual(refreshToken, result.RefreshToken);
        }

        [Fact]
        public async Task RefreshTokenAsync_MissingToken_ReturnsFail()
        {
            var result = await _authService.RefreshTokenAsync(null);
            Assert.False(result.Success);
            Assert.Equal("Refresh token is missing.", result.ErrorMessage);
        }

        [Fact]
        public async Task RefreshTokenAsync_InvalidOrExpiredToken_ReturnsFail()
        {
            // User not found by token
            _mockUserManager.Setup(um => um.Users).Returns(new List<ApplicationUser>().AsQueryable());
            var result1 = await _authService.RefreshTokenAsync("non_existent_token");
            Assert.False(result1.Success);
            Assert.Equal("Invalid or expired refresh token.", result1.ErrorMessage);

            // Token expired
            var expiredUser = new ApplicationUser { RefreshToken = "expired_token", RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(-1) };
            _mockUserManager.Setup(um => um.Users).Returns(new List<ApplicationUser> { expiredUser }.AsQueryable());
            var result2 = await _authService.RefreshTokenAsync("expired_token");
            Assert.False(result2.Success);
            Assert.Equal("Invalid or expired refresh token.", result2.ErrorMessage);
        }

        [Fact]
        public async Task RefreshTokenAsync_UpdateUserFails_ReturnsFail()
        {
            var refreshToken = "valid-refresh-token";
            var appUser = new ApplicationUser { Id = "1", RefreshToken = refreshToken, RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(1) };
            _mockUserManager.Setup(um => um.Users).Returns(new List<ApplicationUser> { appUser }.AsQueryable());
            _mockUserManager.Setup(um => um.GetRolesAsync(appUser)).ReturnsAsync(new List<string>());
            _mockUserManager.Setup(um => um.UpdateAsync(It.IsAny<ApplicationUser>())).ReturnsAsync(IdentityResult.Failed(new IdentityError{Description = "DB update error"}));

            var result = await _authService.RefreshTokenAsync(refreshToken);

            Assert.False(result.Success);
            Assert.Equal("Failed to update user session.", result.ErrorMessage);
            Assert.Contains("DB update error", result.Errors);
        }

        // --- LogoutAsync Tests ---
        [Fact]
        public async Task LogoutAsync_WithValidRefreshToken_ClearsTokenAndReturnsSuccess()
        {
            var refreshToken = "user-refresh-token";
            var appUser = new ApplicationUser { RefreshToken = refreshToken };
            _mockUserManager.Setup(um => um.Users).Returns(new List<ApplicationUser> { appUser }.AsQueryable());
            _mockUserManager.Setup(um => um.UpdateAsync(appUser)).ReturnsAsync(IdentityResult.Success);

            var result = await _authService.LogoutAsync(refreshToken);

            Assert.True(result.Success);
            Assert.Equal("Logged out successfully.", result.Data);
            Assert.Null(appUser.RefreshToken); // Check if token was cleared
        }

        [Fact]
        public async Task LogoutAsync_WithInvalidOrMissingRefreshToken_StillReturnsSuccessButDoesNothing()
        {
            _mockUserManager.Setup(um => um.Users).Returns(new List<ApplicationUser>().AsQueryable());

            var result1 = await _authService.LogoutAsync("non_existent_token");
            Assert.True(result1.Success);
            Assert.Equal("Logged out successfully.", result1.Data);

            var result2 = await _authService.LogoutAsync(null);
            Assert.True(result2.Success);
            Assert.Equal("Logged out successfully.", result2.Data);
        }
    }
}
