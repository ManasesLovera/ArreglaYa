using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models;

/// <summary>
/// Represents a transaction between a client and a company service.
/// </summary>
public class Transaction
{
    /// <summary>
    /// Gets or sets the unique identifier for the transaction.
    /// </summary>
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the ID of the client involved in the transaction.
    /// </summary>
    public string? ClientId { get; set; }

    /// <summary>
    /// Gets or sets the client involved in the transaction.
    /// </summary>
    public Client? Client { get; set; }

    /// <summary>
    /// Gets or sets the ID of the company service associated with the transaction.
    /// </summary>
    public int CompanyServiceId { get; set; }

    /// <summary>
    /// Gets or sets the company service associated with the transaction.
    /// </summary>
    public CompanyService? CompanyService { get; set; }
}