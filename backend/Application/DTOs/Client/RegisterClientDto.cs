using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Client
{
    public record RegisterClientDto
    (
        string Email,
        string Username,
        string Password,
        string FullName
    );
}
