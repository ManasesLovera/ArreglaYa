using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Common
{
    /// <summary>
    /// Represents a paginated response with metadata.
    /// </summary>
    public class PaginatedResponse<T>
    {
        /// <summary>
        /// The data items for the current page.
        /// </summary>
        public IEnumerable<T> Items { get; }

        /// <summary>
        /// The total number of records available.
        /// </summary>
        public int TotalRecords { get; }

        /// <summary>
        /// The current page index.
        /// </summary>
        public int PageIndex { get; }

        /// <summary>
        /// The number of records per page.
        /// </summary>
        public int PageSize { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PaginatedResponse{T}"/> class.
        /// </summary>
        public PaginatedResponse(IEnumerable<T> items, int totalRecords, int pageIndex, int pageSize)
        {
            Items = items;
            TotalRecords = totalRecords;
            PageIndex = pageIndex;
            PageSize = pageSize;
        }
    }
}
