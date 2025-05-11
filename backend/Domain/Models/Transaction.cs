using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using Domain.Enums;

namespace Domain.Models
{
    public class Transaction
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        public TransactionStatus Status { get; set; } = TransactionStatus.Pending;

        public string? ClientId { get; set; }

        public string? CompanyServiceId { get; set; }

        [ForeignKey(nameof(CompanyServiceId))]
        public CompanyService? CompanyService { get; set; }

        [ForeignKey(nameof(ClientId))]
        public ApplicationUser? Client { get; set; }
    }
}
