using EUSWrapper.Models;
using System.Text.Json;

namespace EUSWrapper.Logic
{
    public static class EUSWrapperLogic
    {
        public static List<BibliotecaGEN> GetBibliotecas()
        {
            string jsonString = File.ReadAllText(".\\Resources\\EUSDemo.json");
            List<BibliotecaEUS> bibliotecasEUS = JsonSerializer.Deserialize<List<BibliotecaEUS>>(jsonString);
            List<BibliotecaGEN> myBibliotecas = new List<BibliotecaGEN>();
            BibliotecaGEN bibliotecaGEN;

            foreach (BibliotecaEUS biblioteca in bibliotecasEUS)
            {
                bibliotecaGEN = new BibliotecaGEN();
                bibliotecaGEN.nombre = biblioteca.documentName;
                bibliotecaGEN.tipo = "Publica";
                bibliotecaGEN.direccion = biblioteca.address;
                bibliotecaGEN.codigoPostal = GetCodigoPostal(biblioteca.postalcode);
                bibliotecaGEN.longitud = biblioteca.lonwgs84;
                bibliotecaGEN.latitud = biblioteca.latwgs84;
                bibliotecaGEN.telefono = GetTelefono(biblioteca.phone);
                bibliotecaGEN.email = biblioteca.email;
                bibliotecaGEN.descripcion = GetDescripcion(biblioteca.documentDescription);
                bibliotecaGEN.nombreLocalidad = biblioteca.municipality;
                bibliotecaGEN.codigoLocalidad = GetCodigoLocalidad(biblioteca.municipality);
                bibliotecaGEN.nombreProvincia = biblioteca.territory;
                bibliotecaGEN.codigoProvincia = GetCodigoPostal(biblioteca.postalcode).Substring(0, 2);

                myBibliotecas.Add(bibliotecaGEN);
            }

            return myBibliotecas;
        }

        public static string GetCodigoPostal(string codigoPostalOriginal)
        {
            return codigoPostalOriginal.Replace(".", string.Empty);
        }

        public static string GetTelefono(string telefonoOriginal)
        {
            string telefonoADevolver = telefonoOriginal.Replace(" ", string.Empty);
            telefonoADevolver = telefonoADevolver.Length != 18 ? telefonoADevolver : telefonoADevolver.Insert(9, "/");

            return telefonoADevolver;
        }

        public static string GetDescripcion(string descripcionOriginal)
        {
            return descripcionOriginal.Split("-")[1].TrimStart(' ');
        }

        public static string GetCodigoLocalidad(string localidad)
        {
            if (CodigosMunicipiosEUS.codigosMunicipiosEuskadi.ContainsKey(localidad))
            {
                return CodigosMunicipiosEUS.codigosMunicipiosEuskadi[localidad];
            }
            else
            {
                Console.WriteLine(localidad);
                return "";
            }
        }
    }    
}
