using Microsoft.AspNetCore.Identity;

namespace Domain.Models
{
    /// <summary>
    /// Represents an application user, extending ASP.NET Core Identity's IdentityUser.
    /// Supports multiple user types: Company, Client, and Admin.
    /// </summary>
    public class ApplicationUser : IdentityUser
    {
        /// <summary>
        /// Gets or sets the full name of the user.
        /// </summary>
        public string FullName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets a value indicating whether the user account is active.
        /// </summary>
        public bool IsActive { get; set; } = true;

        /// <summary>
        /// Gets or sets the current refresh token issued to the user for JWT authentication.
        /// </summary>
        public string? RefreshToken { get; set; }

        /// <summary>
        /// Gets or sets the expiry date and time for the refresh token.
        /// </summary>
        public DateTime RefreshTokenExpiryTime { get; set; }

        #region Company
        /// <summary>
        /// Gets or sets the collection of services offered by the company.
        /// Only applicable when the user is a Company.
        /// </summary>
        public ICollection<CompanyService> CompanyServices { get; set; } = new List<CompanyService>();
        #endregion

        #region Client
        /// <summary>
        /// Gets or sets the collection of transactions associated with the client.
        /// Only applicable when the user is a Client.
        /// </summary>
        public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
        #endregion
    }
}
