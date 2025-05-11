using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using Domain.Enums;

namespace Domain.Models;

/// <summary>
/// Represents a transaction between a client and a company service.
/// </summary>
public class Transaction
{
<<<<<<< HEAD
    public class Transaction
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        public TransactionStatus Status { get; set; } = TransactionStatus.Pending;
=======
    /// <summary>
    /// Gets or sets the unique identifier for the transaction.
    /// </summary>
    [Key]
    public int Id { get; set; }
>>>>>>> dev

    /// <summary>
    /// Gets or sets the ID of the client involved in the transaction.
    /// </summary>
    public string? ClientId { get; set; }

<<<<<<< HEAD
        public string? CompanyServiceId { get; set; }

        [ForeignKey(nameof(CompanyServiceId))]
        public CompanyService? CompanyService { get; set; }

        [ForeignKey(nameof(ClientId))]
        public ApplicationUser? Client { get; set; }
    }
}
=======
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
>>>>>>> dev
