using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Admin
{
    public class RegisterResponse
    {
        public bool HasError { get; set; }

        public string? ResultMessage { get; set; }
    }
}
