using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Client
{
    public record ClientDto 
    (
        string Id,
        string Email,
        string Username,
        string FullName
    );
}
