using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    /// <summary>
    /// Base controller providing common dependencies for all controllers.
    /// Includes AutoMapper for DTO mapping.
    /// </summary>
    public class BaseController : ControllerBase
    {
        /// <summary>
        /// AutoMapper instance for DTO mappings.
        /// </summary>
        protected readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseController"/> class.
        /// </summary>
        /// <param name="mapper">The AutoMapper instance.</param>
        public BaseController(IMapper mapper)
        {
            _mapper = mapper;
        }
    }
}
