using Application.Mapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.IOC
{
    public static class AddApplication
    {
        public static void AddAplicationLayer(this IServiceCollection services)
        {
            #region Mapper
            services.AddAutoMapper(typeof(MapperProfile));
            #endregion
        }
    }
}
