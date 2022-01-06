using EUSWrapper.Logic;
using EUSWrapper.Models;
using Microsoft.AspNetCore.Mvc;

namespace EUSWrapper.Controllers
{
    [Route("euswrapper/eus")]
    [ApiController]
    public class BibliotecaGENsController : ControllerBase
    {
        // GET: euswrapper/eus
        [HttpGet]
        public IEnumerable<BibliotecaGEN> Get()
        {
            return EUSWrapperLogic.GetBibliotecas();
        }
    }
}
