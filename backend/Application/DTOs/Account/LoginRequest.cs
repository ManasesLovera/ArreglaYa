using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Account
{
    public sealed class LoginRequest
    {
        /// <summary>
        /// The user's email address which acts as a user name.
        /// </summary>
        public required string Email { get; init; }

        /// <summary>
        /// The user's password.
        /// </summary>
        public required string Password { get; init; }
    }
}
