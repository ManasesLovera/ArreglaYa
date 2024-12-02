using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Admin
{
    public class RegisterResponse
    {
        public string? Id { get; set; }

        public string? Email { get; set; }

        public string? Username { get; set; }

        public string? FullName { get; set; }
        
        public string? ResultMessage { get; set; }
    }
}
