using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseController : ControllerBase
    {
        private IMapper _mapper;
        protected IMapper Mapper => _mapper ?? (_mapper = HttpContext.RequestServices.GetService<IMapper>());
    }
}