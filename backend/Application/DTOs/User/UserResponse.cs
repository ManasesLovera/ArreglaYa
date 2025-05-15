using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.User
{
    public class UserResponse
    {
        public string Id { get; set; } = String.Empty;
        public string FullName { get; set; } = String.Empty;
        public string UserName { get; set; } = String.Empty;
        public string Email { get; set; } = String.Empty;
        public bool IsActive { get; set; }
        public string[] Roles { get; set; } = Array.Empty<string>();
    }
}