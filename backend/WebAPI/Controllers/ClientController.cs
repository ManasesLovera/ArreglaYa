using Application.DTOs.Admin;
using Application.DTOs.Client;
using AutoMapper;
using Domain;
using Domain.Interfaces;
using Domain.Models;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientController : ControllerBase
    {
        private readonly SignInManager<IUser> _signInManager;
        private readonly UserManager<IUser> _userManager;
        private readonly IMapper _mapper;
        private readonly IValidator<RegisterClientDto> _validator;

        public ClientController(
            SignInManager<IUser> signInManager, 
            UserManager<IUser> userManager, 
            IMapper mapper, IValidator<RegisterClientDto> validator)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _mapper = mapper;
            _validator = validator;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ClientDto>> GetByIdClient([FromRoute] string id)
        {
            try
            {
             var client = await _userManager.FindByIdAsync(id);

             if (client == null)
             { 
                return NotFound(new ClientResult(false, null, $"this Id {id} not found"));
             }
             var clientDto = _mapper.Map<ClientDto>(client);
             return Ok(new ClientResult(true, clientDto, "Query successful"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new AdminResult(false, null, $"Internal Server Error: {ex.Message}"));
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateClient([FromBody] RegisterClientDto request)
        {

            var result = await _validator.ValidateAsync(request);
            if (!result.IsValid)
            {
                return BadRequest(result.Errors);
            }

            var userWithEmail = await _userManager.FindByEmailAsync(request.Email);
            if (userWithEmail != null)
            {
                return BadRequest(new ClientResult(false, null, $"This email {request.Email} is taken"));
            }

            var username = await _userManager.FindByNameAsync(request.Username);
            if (username != null)
            {
                return BadRequest(new ClientResult(false,null, $"this username {request.Username} is taken"));
            }

            Client client = new()
            {
                UserName = request.Username,
                Email = request.Email,
                FullName = request.FullName
            };

            var resultUser = await _userManager.CreateAsync(client, request.Password);
            if (!resultUser.Succeeded)
            {
                return BadRequest(resultUser.Errors);
            }

            var response = _mapper.Map<RegisterResponse>(client);
            return CreatedAtAction(nameof(GetByIdClient), new {Id = client.Id}, response);
        }

        [HttpDelete("{Id}")]
        public async Task<ActionResult<ClientDto>> DeleteByIdClient([FromRoute] string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                await _userManager.DeleteAsync(user);
                return NoContent();
            }

            return NotFound(new ClientResult(false, null, $"this Id {id} not found"));
        }

    }
}
