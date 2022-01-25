using APICarga.Logic;
using APICarga.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Text.Json;

namespace APICarga.Controllers
{
    [Route("carga/bibliotecas")]
    [ApiController]
    public class CargaController : Controller
    {
        // POST: carga/bibliotecas
        [HttpPost]
        public async Task<string> Post(BibliotecasSeleccionadas bibl)
        {
            string message = string.Empty;
            HttpClient client = new HttpClient();
            List<BibliotecaGEN> bibliotecasAnyadir = new List<BibliotecaGEN>();

            if (bibl.Cat)
            {
                client.BaseAddress = new Uri("https://localhost:7086/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                Task<HttpResponseMessage> responseTask = client.GetAsync("catwrapper/cat");
                responseTask.Wait();

                HttpResponseMessage response = responseTask.Result;
                if (response.IsSuccessStatusCode)
                {
                    List<BibliotecaGEN> bibliotecasCAT = new List<BibliotecaGEN>();
                    Task<string> resultTask = response.Content.ReadAsStringAsync();
                    resultTask.Wait();
                    string result = resultTask.Result;
                    if (result is not "")
                    {
                        Console.WriteLine(result);
                        bibliotecasCAT = JsonSerializer.Deserialize<List<BibliotecaGEN>>(result);
                    }
                    message += "Cat successful";

                    bibliotecasAnyadir.AddRange(bibliotecasCAT);
                }
                else
                {
                    Console.WriteLine("Internal Server Error");
                }
            }
            if (bibl.Eus)
            {
                client = new HttpClient();
                client.BaseAddress = new Uri("https://localhost:7266/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                Task<HttpResponseMessage> responseTask = client.GetAsync("euswrapper/eus");
                responseTask.Wait();

                HttpResponseMessage response = responseTask.Result;
                if (response.IsSuccessStatusCode)
                {
                    List<BibliotecaGEN> bibliotecasEUS = new List<BibliotecaGEN>();
                    Task<string> resultTask = response.Content.ReadAsStringAsync();
                    resultTask.Wait();
                    string result = resultTask.Result;
                    if (result is not "")
                    {
                        bibliotecasEUS = JsonSerializer.Deserialize<List<BibliotecaGEN>>(result);
                    }
                    message += "Eus successful";

                    bibliotecasAnyadir.AddRange(bibliotecasEUS);
                }
                else
                {
                    Console.WriteLine("Internal Server Error");
                }
            }
            if (bibl.Val)
            {
                client = new HttpClient();
                client.BaseAddress = new Uri("https://localhost:7017/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                Task<HttpResponseMessage> responseTask = client.GetAsync("cvwrapper/cv");
                responseTask.Wait();

                HttpResponseMessage response = responseTask.Result;
                if (response.IsSuccessStatusCode)
                {
                    List<BibliotecaGEN> bibliotecasVAL = new List<BibliotecaGEN>();
                    Task<string> resultTask = response.Content.ReadAsStringAsync();
                    resultTask.Wait();
                    string result = resultTask.Result;
                    if (result is not "")
                    {
                        bibliotecasVAL = JsonSerializer.Deserialize<List<BibliotecaGEN>>(result);
                    }
                    message += "Val successful";

                    bibliotecasAnyadir.AddRange(bibliotecasVAL);
                }
                else
                {
                    Console.WriteLine("Internal Server Error");
                }
            }

            CargaLogic.CargarBibliotecas(bibliotecasAnyadir);

            Console.WriteLine(message);

            return message;
        }
    }
}
