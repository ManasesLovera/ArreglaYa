using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models;

/// <summary>
/// Represents a service offered by a company.
/// </summary>
public class CompanyService
{
    /// <summary>
    /// Gets or sets the unique identifier for the service.
    /// </summary>
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the name of the service.
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// Gets or sets the description of the service.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Gets or sets the price of the service.
    /// </summary>
    public decimal Price { get; set; }

    /// <summary>
    /// Gets or sets the ID of the company offering the service.
    /// </summary>
    public string? CompanyId { get; set; }

    /// <summary>
    /// Gets or sets the company offering the service.
    /// </summary>
    public Company? Company { get; set; }

    /// <summary>
    /// Gets or sets the collection of transactions associated with the service.
    /// </summary>
    public ICollection<Transaction>? Transactions { get; set; }
}
