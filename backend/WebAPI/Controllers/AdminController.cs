using Application.DTOs.Admin;
using Application.Interfaces.Services;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using WebAPI.Validation.Admin;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminService;
        private readonly IValidator<RegisterRequest> _validator;
        public AdminController(IAdminService adminService, IValidator<RegisterRequest> validator )
        {
            _adminService = adminService;
            _validator = validator;
        }

        [HttpGet()]
        public async Task<ActionResult<AdminDTos>> GetById([FromQuery]string id)
        {
            var adminId = await _adminService.GetByIdAsync(id);

            return adminId == null ? NotFound() : Ok(adminId);
        }

        [HttpPost("add")]
        public async Task<ActionResult<AdminDTos>> Create(RegisterRequest request)
        {
            var result = await _validator.ValidateAsync(request);

            if (!result.IsValid)
            {
                return BadRequest(result.Errors);
            }

            var adminCreate = await _adminService.RegisterAsync(request);

            return CreatedAtAction(nameof(GetById), new {Id = adminCreate.Id}, adminCreate);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(string id)
        {
            var admin = await _adminService.DeleteAsync(id);
            return admin == null ? NotFound("User not found") : NoContent();
        }
    }
}
