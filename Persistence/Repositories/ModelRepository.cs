using System.Threading.Tasks;
using Core.Domain;
using Core.Interfaces;

namespace Persistence.Repositories
{
    public class ModelRepository : IModelRepository
    {
        private readonly DataContext _context;

        public ModelRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<Model> Details(int id)
        {
            return await _context.Models.FindAsync(id);
        }
    }
}