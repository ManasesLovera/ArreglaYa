using Application.Interfaces.Repository;
using Domain.Models;
using Infrastructure.Data;

namespace Infrastructure.Repositories
{
    /// <summary>
    /// Repository for managing user-specific data access operations.
    /// Inherits common CRUD operations from <see cref="GenericRepository{T}"/>.
    /// </summary>
    public class UserRepository : GenericRepository<ApplicationUser>, IUserRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserRepository"/> class.
        /// </summary>
        /// <param name="context">The application database context.</param>
        public UserRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
