using EUSWrapper.Logic;
using EUSWrapper.Models;
using Microsoft.AspNetCore.Mvc;

namespace EUSWrapper.Controllers
{
    [Route("api/euswrapper/obtenerBibliotecasEus")]
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
