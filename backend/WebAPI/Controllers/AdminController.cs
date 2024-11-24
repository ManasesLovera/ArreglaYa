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
        private readonly IValidator<UpdateAdminDTos> _validatorUpdate;
        public AdminController(IAdminService adminService, IValidator<RegisterRequest> validator, IValidator<UpdateAdminDTos> validatorUpdate )
        {
            _adminService = adminService;
            _validator = validator;
            _validatorUpdate = validatorUpdate;
        }

        [HttpGet("all")]
        public async Task<IEnumerable<AdminDTos>> GetAll() => await _adminService.GetAllAsync();

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

            return Ok(adminCreate);
        }

        [HttpPatch("{id}")]
        public async Task<ActionResult<AdminDTos>> Update(string id, [FromBody] UpdateAdminDTos updateAdminDTos)
        {
            var result = await _validatorUpdate.ValidateAsync(updateAdminDTos);

            if (!result.IsValid)
            {
                return BadRequest(result.Errors);
            }
            var adminUpdate = await _adminService.UpdateAsync(id,updateAdminDTos);
            
            return adminUpdate == null ? NotFound("No admin found with this id") : Ok(adminUpdate);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(string id)
        {
            var admin = await _adminService.DeleteAsync(id);
            return admin == null ? NotFound() : NoContent();
        }
    }
}
