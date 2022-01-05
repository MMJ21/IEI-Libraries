using Microsoft.AspNetCore.Mvc;

namespace APIBusqueda.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BusquedaController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

        private readonly ILogger<BusquedaController> _logger;

        public BusquedaController(ILogger<BusquedaController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetBibliotecas")]
        public IEnumerable<Busqueda> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new Busqueda
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}