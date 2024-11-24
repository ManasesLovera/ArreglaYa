using Domain.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Repository
{
    public interface IAdminRepository
    {
        Task<Admin> FindByNameAsync(string username);

        Task<IdentityResult> CreateAsync(Admin admin, string password);

        Task<IEnumerable<Admin>> GetAllAsync();

        Task<Admin> GetByIdAsync(string id);

        Task DeleteAsync(Admin admin);

        Task<IdentityResult> UpdateAsync(Admin admin);

        Task<Admin> FindByEmailAsync(string email);

        Task<IdentityResult> ConfirmEmailAsync(Admin adminId, string confirmationToken);

        Task<SignInResult> PasswordSignInAsync(string username, string password,bool isPersistent, bool lockoutOnFailure);
    }
}
