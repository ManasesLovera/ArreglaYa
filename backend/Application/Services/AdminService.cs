using Application.DTOs.Admin;
using Application.Interfaces.Repository;
using Application.Interfaces.Services;
using AutoMapper;
using Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class AdminService : IAdminService
    {
        private readonly SignInManager<Admin> _signInManager;
        private readonly UserManager<Admin> _userManager;
        private readonly IMapper _mapper;

        public AdminService(SignInManager<Admin> signInManager, UserManager<Admin> userManager, IMapper mapper)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _mapper = mapper;
        }
        public async Task<RegisterResponse> RegisterAsync(RegisterRequest request)
        {
            RegisterResponse response = new();

            var email = await _userManager.FindByEmailAsync(request.Email);

            if (email != null)
            {
                response.ResultMessage = $"This email is taken {email}";
                return response;
            }

            var username = await _userManager.FindByNameAsync(request.Username);
            if (username != null)
            { 
                response.ResultMessage = $"This user is taken {username}"; 
                return response;
            }
            var admin = new Admin
            {   
                Email = request.Email,
                UserName = request.Username,
                FullName = request.FullName
            };

            var result = await _userManager.CreateAsync(admin, request.Password);
            if (!result.Succeeded)
            {
                 response.ResultMessage = "An error ocurred trying to registed the user";
                return response;
            }

            var responseDto = _mapper.Map<RegisterResponse>(admin); 

            return responseDto;
        }

        public async Task<AdminDTos> GetByIdAsync(string id)
        {
            var adminId = await _userManager.FindByIdAsync(id);
            if (adminId != null)
            {
                var adminDto = _mapper.Map<AdminDTos>(adminId);
                return adminDto;
            }
            return null;
        }

        public async Task<AdminDTos> DeleteAsync(string id)
        {
            var adminId = await _userManager.FindByIdAsync(id);

            if (adminId != null)
            {
                await _userManager.DeleteAsync(adminId);
                var adminDto = _mapper.Map<AdminDTos>(adminId);
                return adminDto;
            }

            return null;
        } 
    }
}
 