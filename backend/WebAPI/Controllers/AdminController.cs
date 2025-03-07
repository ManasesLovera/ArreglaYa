using AutoMapper;
using Domain.Models;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Domain;
using Application.DTOs.Account;
using Domain.Interfaces;
using Application.DTOs.Common;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : BaseController
    {
        private readonly SignInManager<IUser> _signInManager;
        private readonly UserManager<IUser> _userManager;
        private readonly IMapper _mapper;

        public AdminController(SignInManager<IUser> signInManager, UserManager<IUser> userManager, IMapper mapper, 
            IValidator<RegisterEntityRequest> validator) : base(validator)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _mapper = mapper;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<RegisterEntityResponse>> GetAdminById([FromRoute]string id)
        {
            try
            {
                var admin = await _userManager.FindByIdAsync(id);
                if (admin == null)
                {
                    return NotFound(ApiResponse<string>.ErrorResponse("Admin not found"));
                }
                var adminDto = _mapper.Map<RegisterEntityResponse>(admin);
                return Ok(ApiResponse<RegisterEntityResponse>.SuccessResponse(adminDto));
            }
            catch (Exception ex)
            {
                return StatusCode(500,  ApiResponse<string>.ErrorResponse($"Internal Server Error: {ex.Message}"));
            }
        }

        [HttpPost]
        public async Task<ActionResult<RegisterEntityResponse>> Create(RegisterEntityRequest request)
        {
            var result = await _validator.ValidateAsync(request);

            if (!result.IsValid)
            {
                return BadRequest(result.Errors);
            }

            var email = await _userManager.FindByEmailAsync(request.Email);

            if (email != null)
            {
                return NotFound(ApiResponse<string>.ErrorResponse($"This email is taken {email}"));
            }

            var username = await _userManager.FindByEmailAsync(request.Username);

            if (username != null)
            {
                return BadRequest(ApiResponse<string>.ErrorResponse($"This user is taken {username}"));
            }

            Admin admin = new Admin
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

            var responseDto = _mapper.Map<RegisterEntityResponse>(admin);

            return CreatedAtAction(nameof(GetAdminById),new {Id = admin.Id}, responseDto);

        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(string id)
        {
            var adminId = await _userManager.FindByIdAsync(id);
            if (adminId != null)
            {
                await _userManager.DeleteAsync(adminId);
                var adminDto = _mapper.Map<RegisterEntityResponse>(adminId);
                return NoContent();
            }
            return NotFound(ApiResponse<string>.ErrorResponse($"{adminId} not found"));
        }
    }
}


