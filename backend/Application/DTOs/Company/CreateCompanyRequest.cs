using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Company
{
    public record CreateCompanyRequest
    (
        string Username,
        string Fullname,
        string Email,
        string Password
    );
}
