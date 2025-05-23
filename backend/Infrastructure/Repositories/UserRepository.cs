using Application.Interfaces.Repository;
using Domain.Models;
using Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class UserRepository : GenericRepository<ApplicationUser>, IUserRepository
    {
        public UserRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
