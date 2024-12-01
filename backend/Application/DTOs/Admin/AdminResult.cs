using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Admin
{
    public record AdminResult
    (
        bool IsSuccessful,
        Domain.Models.Admin Admin,
        string Message
    );
}
