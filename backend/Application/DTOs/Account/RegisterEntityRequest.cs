using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Account
{
    public record RegisterEntityRequest
    (
        string Email,
        string Username,
        string Password,
        string FullName
    );
}
