using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Interfaces;

namespace Domain.Models;

/// <summary>
/// Represents a company user with identity information and associated services.
/// </summary>
public class Company : IdentityUser, IUser
{
    /// <summary>
    /// Gets or sets the full name of the company.
    /// </summary>
    public string FullName { get; set; } = string.Empty;

    /// <summary>
    /// Gets the collection of services offered by the company.
    /// </summary>
    public ICollection<CompanyService>? CompanyServices { get; }
}

