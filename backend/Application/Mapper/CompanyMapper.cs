using Application.DTOs.Company;
using AutoMapper;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Mapper
{
    public class CompanyMapper : Profile
    {
        public CompanyMapper()
        {
            CreateMap<CreateCompanyRequest, Company>();
            CreateMap<Company, CompanyResponse>();
        }
    }
}
