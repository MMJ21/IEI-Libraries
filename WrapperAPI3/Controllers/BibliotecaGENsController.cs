using CVWrapper.Logic;
using CVWrapper.Models;
using Microsoft.AspNetCore.Mvc;

namespace CVWrapper.Controllers
{
    [Route("api/cvwrapper/obtenerBibliotecasCV")]
    [ApiController]
    public class BibliotecaGENsController : ControllerBase
    {
        // GET: CVwrapper/CV
        [HttpGet]
        public IEnumerable<BibliotecaGEN> Get()
        {
            return CVWrapperLogic.GetBibliotecas();
        }
    }
}
