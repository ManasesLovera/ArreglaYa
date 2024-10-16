using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class Transaction
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("ClientId")]
        public int ClientId { get; set; }

        public Client Client { get; set; }

        [ForeignKey("CompanyServiceId")]
        public int CompanyServiceId { get; set; }

        public CompanyService CompanyService { get; set; }
    }
}
