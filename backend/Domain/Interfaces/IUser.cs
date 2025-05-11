using System.ComponentModel.DataAnnotations;

namespace Domain.Interfaces;

/// <summary>
/// Represents a user with basic identity information.
/// </summary>
public interface IUser
{
    /// <summary>
    /// Gets or sets the full name of the user.
    /// </summary>
    [MaxLength(100)]
    string FullName { get; set; }
}