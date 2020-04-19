using System.Threading.Tasks;
using Core.Domain;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _context;

        public UserRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<User> Details(string username)
        {
            return await _context.Users
                .Include(u => u.Vehicles)
                .SingleOrDefaultAsync(u => u.UserName == username);
        }
    }
}