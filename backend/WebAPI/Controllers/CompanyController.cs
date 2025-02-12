using Application.DTOs.Common;
using Application.DTOs.Company;
using Application.Interfaces.Repository;
using AutoMapper;
using Domain.Models;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    /// <summary>
    /// Controller for managing company-related operations.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyController : ControllerBase
    {
        private readonly ICompanyRepository _companyRepo;
        private readonly IValidator<CreateCompanyRequest> _validator;
        private readonly UserManager<BaseUser> _userManager;
        private readonly IMapper _mapper;

        public CompanyController(ICompanyRepository companyRepo, IValidator<CreateCompanyRequest> validator, UserManager<BaseUser> userManager, IMapper mapper)
        {
            _companyRepo = companyRepo;
            _validator = validator;
            _userManager = userManager;
            _mapper = mapper;
        }

        /// <summary>
        /// Retrieves paginated list of companies.
        /// </summary>
        /// <returns>A IEnumerable with all companies.</returns>
        [HttpGet]
        public async Task<PaginatedResponse<CompanyResponse>> GetAllAsync([FromQuery] PaginationQuery query)
        {
            var companies = await _companyRepo.GetAllAsync(query.PageIndex, query.PageSize);
            var totalRecords = await _companyRepo.GetTotalCountAsync();
            return new PaginatedResponse<CompanyResponse>(
                _mapper.Map<IEnumerable<CompanyResponse>>(companies),
                totalRecords,
                query.PageIndex,
                query.PageSize
            );
        }

        /// <summary>
        /// Retrieves a company by its ID.
        /// </summary>
        /// <param name="id">The ID of the company.</param>
        [HttpGet("id")]
        public async Task<ActionResult<CompanyResponse>> GetByIdAsync(string id)
        {
            var company = await _companyRepo.GetByIdAsync(id);
            if (company == null)
            {
                return NotFound();
            }
            return Ok(_mapper.Map<CompanyResponse>(company));
        }

        /// <summary>
        /// Creates a new company.
        /// </summary>
        /// <param name="companyRequest">The request data for creating a company.</param>
        [HttpPost]
        public async Task<ActionResult<CompanyResult>> CreateCompany([FromBody] CreateCompanyRequest companyRequest)
        {
            try
            {
                var validationResult = await _validator.ValidateAsync(companyRequest);
                if (!validationResult.IsValid)
                {
                    return BadRequest(new CompanyResult(
                            false, null, string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage))
                        ));
                }

                var existingUser = await _userManager.FindByEmailAsync(companyRequest.Email);
                if (existingUser != null)
                {
                    return Conflict(new CompanyResult(
                            false, null, "Email is already in use."
                        ));
                }

                var company = _mapper.Map<Company>(companyRequest);
                var result = await _userManager.CreateAsync(company, companyRequest.Password);
                if (!result.Succeeded)
                {
                    return BadRequest(new CompanyResult(
                        false,
                        null,
                        string.Join("; ", result.Errors.Select(e => e.Description))
                    ));
                }

                var companyResponse = _mapper.Map<CompanyResponse>(company);
                return CreatedAtAction(nameof(GetByIdAsync), new { id = company.Id }, 
                    new CompanyResult(true, companyResponse, "Company created successfully."));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new CompanyResult(false, null, ex.Message));
            }
        }

        /// <summary>
        /// Deletes a company by its ID.
        /// </summary>
        /// <param name="id">The ID of the company to delete.</param>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCompany(string id)
        {
            var company = await _companyRepo.GetByIdAsync(id);
            if (company == null)
            {
                return NotFound();
            }

            await _companyRepo.DeleteAsync(company.Id);
            return NoContent();
        }

        /// <summary>
        /// Updates the password of a company.
        /// </summary>
        /// <param name="id">The ID of the company.</param>
        /// <param name="request">The request containing the old and new passwords.</param>
        [HttpPut("{id}/password")]
        public async Task<IActionResult> UpdatePassword([FromRoute] string id, [FromBody] UpdatePasswordRequest request)
        {
            var company = await _companyRepo.GetByIdAsync(id);
            if (company == null)
            {
                return NotFound();
            }

            var result = await _userManager.ChangePasswordAsync(company, request.OldPassword, request.NewPassword);
            if (!result.Succeeded)
            {
                return BadRequest(string.Join("; ", result.Errors.Select(e => e.Description)));
            }

            return NoContent();
        }
    }
}
