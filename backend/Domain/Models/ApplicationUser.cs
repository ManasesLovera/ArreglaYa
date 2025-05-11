using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class ApplicationUser : IdentityUser
    {
        /// <summary>
        /// Gets or sets the full name of the user.
        /// </summary>
        public string FullName { get; set; } = String.Empty;

        /// <summary>
        /// Gets or sets a value indicating whether the user account is active.
        /// </summary>
        public bool IsActive { get; set; } = false;

        #region Company
        /// <summary>
        /// Gets the collection of services offered by the company.
        /// </summary>
        public ICollection<CompanyService> CompanyServices { get; set; } = new List<CompanyService>();
        #endregion
        #region Client
        /// <summary>
        /// Gets or sets the collection of transactions associated with the client.
        /// </summary>
        public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
        #endregion
    }
}
