using CATWrapper.Logic;
using CATWrapper.Models;
using Microsoft.AspNetCore.Mvc;

namespace CATWrapper.Controllers
{
    [Route("catwrapper/cat")]
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
