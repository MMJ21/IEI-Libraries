using Microsoft.AspNetCore.Mvc;
using MySqlConnector;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace APIBusqueda.Controllers
{
    [ApiController]
    [Route("api/bibliotecas")]
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

        [HttpGet(Name = "GetBibliotecas")]
        public IActionResult Get(string localidad, string cP, string provincia, string tipo)
        {
            try{
                conectionToDB(); //Initializing DB Connection
            } catch(Exception e){ console.WriteLine(e.getMessage()); return null;}

            string query = "SELECT b.nombre, b.tipo, b.direccion, b.codigoPostal, b.longitud, b.latitud, b.telefono, b.email,b.descripcion FROM mybiblioteca.Biblioteca b " +
                "JOIN mybiblioteca.Localidad l ON (b.codigoLocalidad = l.codigo) " +
                "JOIN mybiblioteca.Provincia p ON (l.codigoProvincia = p.codigo)  " +
                "WHERE 1 = 1 ";

            if (localidad != null) { query += "AND TRIM(UCASE(b.codigoLocalidad)) = TRIM(UCASE(" + localidad + ")) "; }
            if (cP != null) { query += " AND TRIM(UCASE(b.codigoPostal)) = TRIM(UCASE(" + cP + ")) "; }
            if (provincia != null) { query += " AND TRIM(UCASE(l.codigoProvincia)) = TRIM(UCASE(" + provincia + ")) "; }
            if (tipo != null) { query += "AND TRIM(UCASE(b.tipo)) = TRIM(UCASE(" + tipo + ")) "; }

            query += ";";

            using var command = new MySqlCommand(query, connector);
            using var reader = command.ExecuteReader();
            
            string jsonString = '';

            if (reader.HasRows){
                while (reader.Read()){
                    
                    var biblioteca = new Biblioteca(
                            nombre = reader.GetString(0)
                            tipo = reader.GetString(1)
                            direccion = reader.GetString(2)
                            codigoPostal = reader.GetString(3)
                            longitud = reader.GetString(4)
                            latitud = reader.GetString(5)
                            telefono = reader.GetString(6)
                            email = reader.GetString(7)
                            descripcion = reader.GetString(8)
                    );

                    jsonString += JsonSerializer.Serialize(biblioteca)
   
                }
            }

            reader.Close();
            return Ok(jsonString);
   
        }
    }
}