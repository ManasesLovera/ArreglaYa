using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Common
{
    /// <summary>
    /// Represents pagination parameters for queries.
    /// </summary>
    public class PaginationQuery
    {
        /// <summary>
        /// The page number to retrieve.
        /// </summary>
        public int PageIndex { get; set; } = 1;

        /// <summary>
        /// The number of items per page.
        /// </summary>
        public int PageSize { get; set; } = 10;
    }
}
