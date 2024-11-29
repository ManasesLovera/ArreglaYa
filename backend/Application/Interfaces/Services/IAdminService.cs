using Application.DTOs.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Services
{
    public interface IAdminService
    {
        Task<RegisterResponse> RegisterAsync(RegisterRequest request);

        Task<AdminDTos> GetByIdAsync(string id);

        Task<AdminDTos> DeleteAsync(string id);

    }
}
