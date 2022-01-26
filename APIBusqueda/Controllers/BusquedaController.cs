using Microsoft.AspNetCore.Mvc;
using MySqlConnector;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace APIBusqueda.Controllers
{
    [ApiController]
    [Route("api/obtenerBibliotecas")]
    public class BusquedaController : ControllerBase
    {
        public MySqlConnection connector;
        public void conectionToDB()
        {
            // Configuring the DB Connetion
            MySqlConnectionStringBuilder builder = new MySqlConnectionStringBuilder();
            builder.Server = "localhost";
            builder.Database = "mybiblioteca";
            builder.UserID = "root";
            builder.Password = "root";

            connector = new MySqlConnection(builder.ToString());
            connector.Open();
        }

        [HttpPost(Name = "GetBibliotecas")]
        public IActionResult Get(Rootobject data)
        {

            try{
                conectionToDB(); //Initializing DB Connection
            } catch(Exception e){ Console.WriteLine(e.Message); return null;}


            string query = "SELECT b.nombre, b.tipo, b.direccion, b.codigoPostal, b.longitud, b.latitud, b.telefono, b.email,b.descripcion FROM mybiblioteca.Biblioteca b " +
                "JOIN mybiblioteca.Localidad l ON (b.codigoLocalidad = l.codigo) " +
                "JOIN mybiblioteca.Provincia p ON (l.codigoProvincia = p.codigo) WHERE 1 = 1 ";

            if (data.localidad != "") { query += String.Format("AND TRIM(UCASE('{0}')) = TRIM(UCASE(l.nombre)) ", data.localidad ); }
            if (data.cP != "") { query += String.Format("AND TRIM(UCASE(b.codigoPostal)) = TRIM(UCASE('{0}')) ",data.cP); }
            if (data.provincia != "") { query += String.Format("AND TRIM(UCASE(p.nombre)) = TRIM(UCASE('{0}')) ",data.provincia); }
            if (data.tipo != "") { query += String.Format("AND TRIM(UCASE(b.tipo)) = TRIM(UCASE('{0}')) ",data.tipo); }

            query += ";";

            using var command = new MySqlCommand(query, connector);
            using var reader = command.ExecuteReader();

            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
            };

            string jsonString = "";
            var response = new List<Biblioteca>();

            if (reader.HasRows){
                while (reader.Read()){
                    
                    Biblioteca biblioteca = new Biblioteca(
                            reader.GetString(0),
                            reader.GetString(1),
                            reader.GetString(2),
                            reader.GetString(3),                            
                            reader.GetString(4),
                            reader.GetString(5),
                            reader.GetString(6),
                            reader.GetString(7),
                            reader.GetString(8)
                    );
                    jsonString += JsonSerializer.Serialize(biblioteca, options)+ ",";
                    response.Add(biblioteca);

                }
            }
           
            reader.Close();
            connector.Close();

            return new JsonResult(response);

        }

        // Clase adicional que contiene los datos de entrada para POST
        public class Rootobject
        { 
           public string localidad { get; set; }
           public string cP { get; set; }
           public string provincia { get; set; }
           public string tipo { get; set; }
        }
    }
}