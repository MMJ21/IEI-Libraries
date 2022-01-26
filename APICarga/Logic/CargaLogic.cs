using APICarga.Models;
using MySql.Data.MySqlClient;
namespace APICarga.Logic
{
    public class CargaLogic
    {
        public static void CargarBibliotecas(List<BibliotecaGEN> bibliotecas)
        {
            DropTableFromDatabase();
            CreateTableInDatabase();
            SendToDatabase(bibliotecas);
        }

        private static void SendToDatabase(List<BibliotecaGEN> bibliotecas)
        {
            try
            {
                // Connection String for the Database
                MySqlConnectionStringBuilder builder = new MySqlConnectionStringBuilder();
                builder.Server = "localhost";
                builder.Database = "mybiblioteca";
                builder.UserID = "root";
                builder.Password = "root";

                // Opening connection with the Database
                using var connection = new MySqlConnection(builder.ToString());
                connection.Open();

                string commandStringProvincia = "";
                string commandStringLocalidad = "";
                string commandStringBiblioteca = "";                               

                // Inserting into Database
                foreach (BibliotecaGEN biblioteca in bibliotecas)
                {
                    commandStringProvincia += "INSERT IGNORE INTO mybiblioteca.Provincia VALUES ('" +
                        biblioteca.nombreProvincia + "', '" +
                        biblioteca.codigoProvincia + "');\n";

                    commandStringLocalidad += "INSERT IGNORE INTO mybiblioteca.Localidad VALUES ('" +
                        biblioteca.nombreLocalidad + "', '" +
                        biblioteca.codigoLocalidad + "', '" +
                        biblioteca.codigoProvincia + "');\n";

                    commandStringBiblioteca += "INSERT INTO mybiblioteca.Biblioteca VALUES ('" +
                        biblioteca.nombre + "', '" +
                        biblioteca.tipo + "', '" +
                        biblioteca.direccion + "', '" +
                        biblioteca.codigoPostal + "', '" +
                        biblioteca.longitud + "', '" +
                        biblioteca.latitud + "', '" +
                        biblioteca.telefono + "', '" +
                        biblioteca.email + "', '" +
                        biblioteca.descripcion + "', '" +
                        biblioteca.codigoLocalidad + "');\n";
                }

                string[] commandArray = { commandStringProvincia, commandStringLocalidad, commandStringBiblioteca };
                int i = 0;
                MySqlDataReader reader = null;
                MySqlCommand command = null;

                while (i < 3)
                {
                    if (i == 0)
                    {
                        command = new MySqlCommand(commandArray[i++], connection);
                    }
                    else
                    {
                        command.CommandText = commandArray[i++];
                    }
                    reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        Console.WriteLine($"{reader.GetString(0)}");
                    }
                    reader.Close();
                }


                Console.WriteLine(":::COMPLETED SUCCESSFULLY!:::");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private static void DropTableFromDatabase()
        {
            try
            {
                // Connection String for the Database
                MySqlConnectionStringBuilder builder = new MySqlConnectionStringBuilder();
                builder.Server = "localhost";
                builder.Database = "mybiblioteca";
                builder.UserID = "root";
                builder.Password = "root";

                // Opening connection with the Database
                using var connection = new MySqlConnection(builder.ToString());
                connection.Open();

                // Dropping Table
                using var command = new MySqlCommand("DROP TABLE mybiblioteca.Biblioteca; DROP TABLE mybiblioteca.Localidad; DROP TABLE mybiblioteca.Provincia;", connection);
                using var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Console.WriteLine($"{reader.GetString(0)}");
                }
                reader.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private static void CreateTableInDatabase()
        {
            try
            {
                // Connection String for the Database
                MySqlConnectionStringBuilder builder = new MySqlConnectionStringBuilder();
                builder.Server = "localhost";
                builder.Database = "mybiblioteca";
                builder.UserID = "root";
                builder.Password = "root";

                // Opening connection with the Database
                using var connection = new MySqlConnection(builder.ToString());
                connection.Open();

                // Creating Table
                using var command = new MySqlCommand(
                    "CREATE TABLE mybiblioteca.Provincia (" +
                    "nombre VARCHAR(100) NOT NULL," +
                    "codigo VARCHAR(10)," +
                    "constraint PK_Provincia primary key (codigo));" +
                    "CREATE TABLE mybiblioteca.Localidad (" +
                    "nombre VARCHAR(100) NOT NULL," +
                    "codigo VARCHAR(10)," +
                    "codigoProvincia VARCHAR(10)," +
                    "constraint PK_Localidad primary key(codigo)," +
                    "constraint FK_LocalidadInProvincia foreign key(codigoProvincia) references myBiblioteca.Provincia(codigo));" +
                    "CREATE TABLE mybiblioteca.Biblioteca (" +
                    "nombre VARCHAR(500) NOT NULL," +
                    "tipo VARCHAR(45)," +
                    "direccion VARCHAR(200)," +
                    "codigoPostal  VARCHAR(5)," +
                    "longitud VARCHAR(20)," +
                    "latitud VARCHAR(20)," +
                    "telefono VARCHAR(50)," +
                    "email VARCHAR(100)," +
                    "descripcion VARCHAR(5000)," +
                    "codigoLocalidad VARCHAR(10)," +
                    "constraint PK_Biblioteca primary key (nombre, tipo, direccion, codigoPostal)," +
                    "constraint FK_BibliotecaInLocalidad foreign key (codigoLocalidad) references myBiblioteca.Localidad(codigo));",
                    connection);

                using var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Console.WriteLine($"{reader.GetString(0)}");
                }
                reader.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
