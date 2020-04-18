using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Dtos;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Controllers
{
    public class MakesController : BaseController
    {
        private readonly DataContext _context;

        public MakesController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<MakeDto>>> Get()
        {
            var makes = await _context.Makes
                .Include(m => m.Models)
                .ToListAsync();

            return Mapper.Map<List<Make>, List<MakeDto>>(makes);
        }
    }
}