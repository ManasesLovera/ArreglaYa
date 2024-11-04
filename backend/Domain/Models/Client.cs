using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class Client : IdentityUser
    {
        public ICollection<Transaction>? Transactions { get; set; }

        public string? FullName { get; set; }
    }
}
