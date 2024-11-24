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
        private readonly IAdminRepository _adminRepository;
        private readonly IMapper _mapper;

        public AdminService(IAdminRepository adminRepository,IMapper mapper)
        {
            _adminRepository = adminRepository;
            _mapper = mapper;
        }
        public async Task<RegisterResponse> RegisterAsync(RegisterRequest request)
        {
            RegisterResponse response = new ();
            response.HasError = false;

            var email = await _adminRepository.FindByEmailAsync(request.Email);

            if (email != null)
            {
                response.HasError = true;
                response.ResultMessage = $"This email is taken {email}";
                return response;
            }

            var username = await _adminRepository.FindByNameAsync(request.Username);
            if (username != null)
            {
                response.HasError = true;
                response.ResultMessage = $"This user is taken {username}";
                return response;
            }
            request.EmailConfirmed = true;
            //var admin = _mapper.Map<Admin>(request);
            var admin = new Admin
            {
                Email = request.Email,
                UserName = request.Username,
                PhoneNumber = request.Phone,
                EmailConfirmed = request.EmailConfirmed
            };

            var result = await _adminRepository.CreateAsync(admin, request.Password);
            if (!result.Succeeded)
            { 
                response.HasError = true;
                response.ResultMessage = "An error ocurred trying to registed the user";
                return response;
            }
            else
            {
                response.HasError = false;
                response.ResultMessage = $"Your email is {request.Email} and your username is {request.Username}";
                return response;
            }
            //return response;
        }

        public async Task<IEnumerable<AdminDTos>> GetAllAsync()
        {
           var admin = await _adminRepository.GetAllAsync();
            return admin.Select(x => _mapper.Map<AdminDTos>(x));
        }

        public async Task<AdminDTos> GetByIdAsync(string id)
        {
            var adminId = await _adminRepository.GetByIdAsync(id);
            if (adminId != null)
            {
                var adminDto = _mapper.Map<AdminDTos>(adminId);
                return adminDto;
            }
            return null;
        }

        public async Task<AdminDTos> DeleteAsync(string id)
        {
            var adminId = await _adminRepository.GetByIdAsync(id);

            if (adminId != null)
            {
                await _adminRepository.DeleteAsync(adminId);
                var adminDto = _mapper.Map<AdminDTos>(adminId);
                return adminDto;
            }

            return null;
        }

        public async Task<AdminDTos> UpdateAsync(string id, UpdateAdminDTos adminDTos)
        {
            var adminId = await _adminRepository.GetByIdAsync(id);
            if (adminId != null)
            {
                adminId = _mapper.Map<UpdateAdminDTos, Admin>(adminDTos,adminId);
                await _adminRepository.UpdateAsync(adminId);

                var adminDto = _mapper.Map<AdminDTos>(adminId);
                return adminDto;
            }

            return null;
        }
    }
}
 