using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Admin
{
    public record AdminDTos 
    (
        string Id,
        string Email,
        string Username,
        string FullName
    );
}
