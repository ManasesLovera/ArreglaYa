using Application.Interfaces.Repository;
using Infraestructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructure.Repositories
{
    /// <summary>
    /// Generic repository providing CRUD operations for entities.
    /// </summary>
    /// <typeparam name="T">The entity type.</typeparam>
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly ApplicationDbContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="GenericRepository{T}"/> class.
        /// </summary>
        /// <param name="context">The application database context.</param>
        public GenericRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Adds a new entity asynchronously.
        /// </summary>
        /// <param name="entity">The entity to add.</param>
        public virtual async Task AddAsync(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Deletes an entity by its unique identifier asynchronously.
        /// </summary>
        /// <param name="id">The unique identifier of the entity to delete.</param>
        public virtual async Task DeleteAsync(string id)
        {
            var entity = await _context.Set<T>().FindAsync(id);
            if (entity != null)
            {
                _context.Set<T>().Remove(entity);
                await _context.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Retrieves a paginated list of all entities asynchronously.
        /// </summary>
        /// <param name="pageIndex">The page index (default is 1).</param>
        /// <param name="pageSize">The number of items per page (default is 10).</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a list of entities.</returns>
        public virtual async Task<IEnumerable<T>> GetAllAsync(int pageIndex = 1, int pageSize = 10)
        {
            return await _context.Set<T>()
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        /// <summary>
        /// Retrieves the total count of entities asynchronously.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. The task result contains the total count of entities.</returns>
        public virtual async Task<int> GetTotalCountAsync()
        {
            return await _context.Set<T>().CountAsync();
        }

        /// <summary>
        /// Retrieves an entity by its unique identifier asynchronously.
        /// </summary>
        /// <param name="id">The unique identifier of the entity.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the entity if found; otherwise, null.</returns>
        public virtual async Task<T?> GetByIdAsync(string id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        /// <summary>
        /// Updates an existing entity asynchronously.
        /// </summary>
        /// <param name="entity">The entity to update.</param>
        public virtual async Task UpdateAsync(T entity)
        {
            _context.Set<T>().Update(entity);
            await _context.SaveChangesAsync();
        }
    }
}
