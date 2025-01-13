using Application.DTOs.Company;
using Application.Interfaces;
using Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyController : ControllerBase
    {
        private readonly ICompanyRepository _companyRepo;

        public CompanyController(ICompanyRepository companyRepo)
        {
            _companyRepo = companyRepo;
        }

        [HttpGet]
        public async Task<IEnumerable<Company>> GetAllAsync()
        {
            return await _companyRepo.GetAllAsync();
        }

        [HttpPost]
        public async Task<ActionResult<CompanyResult>> CreateCompany([FromBody] CreateCompanyRequest companyRequest)
        {
            try
            {

            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
