using Domain.Models;

namespace Application.Interfaces.Repository
{
    /// <summary>
    /// Repository interface for user-specific data access operations.
    /// Inherits common CRUD operations from <see cref="IGenericRepository{T}"/>.
    /// </summary>
    public interface IUserRepository : IGenericRepository<ApplicationUser>
    {
    }
}
