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
        public int IdCompanyService { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public ICollection<Transaction> Transactions { get; set; }

        [ForeignKey("UserId")]
        public Company Company { get; set; }

    }
}
