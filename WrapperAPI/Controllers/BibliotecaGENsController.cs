using EUSWrapper.Logic;
using EUSWrapper.Models;
using Microsoft.AspNetCore.Mvc;

namespace EUSWrapper.Controllers
{
    [Route("euswrapper/[controller]")]
    [ApiController]
    public class BibliotecaGENsController : ControllerBase
    {
        // GET: euswrapper/<BibliotecaGENsController>
        [HttpGet]
        public IEnumerable<BibliotecaGEN> Get()
        {
            return EUSWrapperLogic.GetBibliotecas();
        }

        // GET euswrapper/<BibliotecaGENsController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST euswrapper/<BibliotecaGENsController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT euswrapper/<BibliotecaGENsController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE euswrapper/<BibliotecaGENsController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
