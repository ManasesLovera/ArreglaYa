using Application.DTOs.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Client
{
    public record ClientResult
    (
        bool IsSuccessful,
        ClientDto Client,
        string Message
    );
}
