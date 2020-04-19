using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Dtos;
using Domain;
using Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Application.Controllers
{
    public class MakesController : BaseController
    {
        private readonly IMakeRepository _makeRepository;

        public MakesController(IMakeRepository makeRepository)
        {
            _makeRepository = makeRepository;
        }

        [HttpGet]
        public async Task<ActionResult<List<MakeDto>>> Get()
        {
            var makes = await _makeRepository.List();

            return Mapper.Map<List<Make>, List<MakeDto>>(makes);
        }
    }
}