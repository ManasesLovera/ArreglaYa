using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class Company : IdentityUser, IUser
    {
        public string FullName { get; set; } = String.Empty;
        public ICollection<CompanyService>? CompanyServices { get; }
    }
}
