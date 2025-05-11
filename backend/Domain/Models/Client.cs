using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Interfaces;

namespace Domain.Models;

/// <summary>
/// Represents a client user with identity information and associated transactions.
/// </summary>
public class Client : IdentityUser, IUser
{
    /// <summary>
    /// Gets or sets the full name of the client.
    /// </summary>
    public string FullName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the collection of transactions associated with the client.
    /// </summary>
    public ICollection<Transaction>? Transactions { get; set; }
}

