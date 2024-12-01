using Application.DTOs.Admin;
using AutoMapper;
using Domain.Models;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using WebAPI.Validation.Admin;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : BaseController
    {
        private readonly SignInManager<Admin> _signInManager;
        private readonly UserManager<Admin> _userManager;
        private readonly IMapper _mapper;

        public AdminController(SignInManager<Admin> signInManager, UserManager<Admin> userManager, IMapper mapper, 
            IValidator<RegisterRequest> validator) : base(validator)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _mapper = mapper;
        }

        [HttpGet()]
        public async Task<ActionResult<AdminDTos>> GetAdminById([FromQuery]string id)
        {
            try
            {
                var admin = await _userManager.FindByIdAsync(id);
                if (admin == null)
                {
                    return NotFound(new AdminResult(false, null, "Admin not found"));
                }
                return Ok(new AdminResult(true, admin, "Query successful"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new AdminResult(false, null, $"Internal Server Error: {ex.Message}"));
            }
        }

        [HttpPost("add")]
        public async Task<ActionResult<AdminDTos>> Create(RegisterRequest request)
        {
            var result = await _validator.ValidateAsync(request);

            if (!result.IsValid)
            {
                return BadRequest(result.Errors);
            }

            var email = await _userManager.FindByEmailAsync(request.Email);

            if (email != null)
            {
                return NotFound(new AdminResult(false, null, $"This email is taken {email}"));
            }

            var username = await _userManager.FindByEmailAsync(request.Username);

            if (username != null)
            {
                return BadRequest(new AdminResult(false,null, $"This user is taken {username}"));
            }

            var admin = new Admin
            {
                Email = request.Email,
                UserName = request.Username,
                FullName = request.FullName
            };

            var resultUser = await _userManager.CreateAsync(admin, request.Password);

            if (!resultUser.Succeeded)
            {
                return BadRequest("An error ocurred trying to registed the user");
            }

            var responseDto = _mapper.Map<RegisterResponse>(admin);

            return CreatedAtAction(nameof(GetAdminById),new {Id = admin.Id}, responseDto);

        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(string id)
        {
            var adminId = await _userManager.FindByIdAsync(id);

            if (adminId != null)
            {
                await _userManager.DeleteAsync(adminId);
                var adminDto = _mapper.Map<AdminDTos>(adminId);
                return NoContent();
            }

            return NotFound("User not found");
        }
    }
}


