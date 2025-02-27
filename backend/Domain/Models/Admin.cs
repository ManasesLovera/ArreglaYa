using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Interfaces;

namespace Domain.Models;

/// <summary>
/// Represents an administrator user with identity and user information.
/// </summary>
public class Admin : IdentityUser, IUser
{
    /// <summary>
    /// Gets or sets the full name of the admin.
    /// </summary>
    public string FullName { get; set; } = string.Empty;
}