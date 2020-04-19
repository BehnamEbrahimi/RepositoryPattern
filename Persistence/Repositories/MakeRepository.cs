using System.Collections.Generic;
using System.Threading.Tasks;
using Domain;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories
{
    public class MakeRepository : IMakeRepository
    {
        private readonly DataContext _context;

        public MakeRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<List<Make>> List()
        {
            return await _context.Makes
                .Include(m => m.Models)
                .ToListAsync();
        }
    }
}