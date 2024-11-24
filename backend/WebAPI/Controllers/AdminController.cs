using Application.DTOs.Admin;
using Application.Interfaces.Services;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminService;

        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
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
            var adminCreate = await _adminService.RegisterAsync(request);

            return adminCreate == null ? BadRequest() : Ok(adminCreate);
        }

        [HttpPatch("{id}")]
        public async Task<ActionResult<AdminDTos>> Update(string id, [FromBody] UpdateAdminDTos updateAdminDTos)
        {            
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
