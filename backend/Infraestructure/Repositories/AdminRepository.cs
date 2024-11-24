using Application.Interfaces.Repository;
using Domain.Models;
using Infraestructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructure.Repositories
{
    public class AdminRepository : IAdminRepository
    {

        private readonly SignInManager<Admin> _signInManager;
        private readonly UserManager<Admin> _userManager;
        private readonly ApplicationDbContext _applicationDbContext;

        public AdminRepository(SignInManager<Admin> signInManager, UserManager<Admin> userManager, ApplicationDbContext applicationDbContext)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _applicationDbContext = applicationDbContext;
        }  

        public async Task<IdentityResult> CreateAsync(Admin admin, string password)
        {
            var adminCreate = await _userManager.CreateAsync(admin,password);
            return adminCreate;
        }

        public async Task<IEnumerable<Admin>> GetAllAsync()
        {  
            var adminAll = await _applicationDbContext.Admins.ToListAsync();
            return adminAll;
        }

        public async Task<Admin> GetByIdAsync(string id)
        {
            var adminId = await _userManager.FindByIdAsync(id);
            return adminId;
        }

        public async Task DeleteAsync(Admin admin)
        {
            var adminDelete = await _userManager.DeleteAsync(admin);
        }

        public async Task<IdentityResult> ConfirmEmailAsync(Admin adminId, string confirmationToken)
        {
            var email = await _userManager.ConfirmEmailAsync(adminId, confirmationToken);
            return email;
        }

        public async Task<Admin> FindByEmailAsync(string email)
        {
            var emailFind = await _userManager.FindByEmailAsync(email);
            return emailFind;
        }

        public async Task<Admin> FindByNameAsync(string username)
        {
            var user = await _userManager.FindByNameAsync(username);
            return user;
        }

        public async Task<SignInResult> PasswordSignInAsync(string username, string password, bool isPersistent, bool lockoutOnFailure)
        {
            var passwordSignIn = await _signInManager.PasswordSignInAsync(username, password,isPersistent,lockoutOnFailure);
            return passwordSignIn;
        }

        public async Task<IdentityResult> UpdateAsync(Admin admin)
        {
           IdentityResult adminUpdate = await _userManager.UpdateAsync(admin);
           return adminUpdate;
        }
    }
}
