using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class CompanyService
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        public required string Name { get; set; }

        public required string Description { get; set; }

        public decimal Price { get; set; }

        public bool IsActive { get; set; }

        public string? CompanyId { get; set; }

        [ForeignKey(nameof(CompanyId))]
        public ApplicationUser? Company { get; set; }

        public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
    }
}
