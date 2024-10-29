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
        public int Id { get; set; }

        public string? Name { get; set; }

        public string? Description { get; set; }

        public decimal Price { get; set; }

        public int UserId { get; set; }

        public User? User { get; set; }

        public int CompanyId { get; set; }

        public Company? Company { get; set; }

        public ICollection<Transaction>? Transactions { get; set; }
    }
}
