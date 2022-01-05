using System.Text.Json;
using System.IO;
using System.Collections.Generic;
using System;

namespace Extractor
{
    public class WrapperEUS
    {
        public static void ExtractEUS(string originFilePath, string destinyFilePath)
        {
            Console.WriteLine("\n::EXTRACTING EUS LIBRARIES::");
            string bibliotecaGenJsonString = string.Empty;
            string resultJson = string.Empty;
            BibliotecaGEN bibliotecaGEN = new BibliotecaGEN();

            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
            };

            string jsonString = File.ReadAllText(originFilePath);
            List<BibliotecaEUS> bibliotecasEUS = JsonSerializer.Deserialize<List<BibliotecaEUS>>(jsonString);

            foreach (BibliotecaEUS biblioteca in bibliotecasEUS)
            {
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

                bibliotecaGenJsonString = JsonSerializer.Serialize(bibliotecaGEN, options);
                resultJson += bibliotecaGenJsonString + ", \n";
            }

            resultJson.TrimEnd('\n');
            resultJson = "[" + resultJson.TrimEnd(new char[] { ',', ' ',  '\n'}) + "]";
            File.WriteAllText(destinyFilePath, resultJson);
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
