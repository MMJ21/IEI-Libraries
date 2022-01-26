using CATWrapper.Logic;
using CATWrapper.Models;
using Microsoft.AspNetCore.Mvc;

namespace CATWrapper.Controllers
{
    [Route("api/catwrapper/obtenerBibliotecasCat")]
    [ApiController]
    public class BibliotecaGENsController : ControllerBase
    {
        // GET: catwrapper/cat
        [HttpGet]
        public IEnumerable<BibliotecaGEN> Get()
        {
            return CATWrapperLogic.GetBibliotecas();
        }
    }
}
