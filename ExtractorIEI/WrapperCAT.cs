using System.Text.Json;
using System.IO;
using System.Collections.Generic;
using System;
using System.Xml;

namespace Extractor
{
    public class WrapperCAT
    {
        public static void ExtractCAT(string originFilePath, string destinyFilePath)
        {
            Console.WriteLine("::EXTRACTING CAT LIBRARIES::");
            string bibliotecaGenJsonString = string.Empty;
            string resultJson = string.Empty;
            BibliotecaGEN bibliotecaGEN = new BibliotecaGEN();

            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
            };

            XmlDocument document = new XmlDocument();
            document.Load(originFilePath);
            XmlNode responseNode = document.DocumentElement;

            foreach(XmlNode node in responseNode.ChildNodes)
            {
                bibliotecaGEN.nombre = (node.SelectSingleNode("nom") != null ? node.SelectSingleNode("nom").InnerText : "Not found").Replace("\'", "´");
                bibliotecaGEN.tipo = (node.SelectSingleNode("categoria") != null ? GetTipo(node.SelectSingleNode("categoria").InnerText) : "Not found").Replace("\'", "´");
                bibliotecaGEN.direccion = (node.SelectSingleNode("via") != null ? node.SelectSingleNode("via").InnerText : "Not found").Replace("\'", "´");
                bibliotecaGEN.codigoPostal = (node.SelectSingleNode("cpostal") != null ? node.SelectSingleNode("cpostal").InnerText : "Not found").Replace("\'", "´");
                bibliotecaGEN.longitud = (node.SelectSingleNode("longitud") != null ? node.SelectSingleNode("longitud").InnerText : "Not found").Replace("\'", "´");
                bibliotecaGEN.latitud = (node.SelectSingleNode("latitud") != null ? node.SelectSingleNode("latitud").InnerText : "Not found").Replace("\'", "´");
                bibliotecaGEN.telefono = (node.SelectSingleNode("telefon1") != null ? GetTelefono(node.SelectSingleNode("telefon1").InnerText) : "Not found").Replace("\'", "´");
                bibliotecaGEN.email = (node.SelectSingleNode("email") != null ? node.SelectSingleNode("email").InnerText : "Not found").Replace("\'", "´");
                bibliotecaGEN.descripcion = (node.SelectSingleNode("propietats") != null ? node.SelectSingleNode("propietats").InnerText : "Not found").Replace("\'", "´");
                bibliotecaGEN.nombreLocalidad = (node.SelectSingleNode("poblacio") != null? node.SelectSingleNode("poblacio").InnerText : "Not found").Replace("\'", "´");
                bibliotecaGEN.codigoLocalidad = ((node.SelectSingleNode("codi_municipi") != null? node.SelectSingleNode("codi_municipi").InnerText : "Not found").Replace("\'", "´"))[2..];
                bibliotecaGEN.nombreProvincia = GetNombreProvincia(bibliotecaGEN.codigoLocalidad);
                bibliotecaGEN.codigoProvincia = GetCodigoProvincia(node.SelectSingleNode("codi_municipi") != null ? node.SelectSingleNode("codi_municipi").InnerText : "Not found");

                bibliotecaGenJsonString = JsonSerializer.Serialize(bibliotecaGEN, options);
                resultJson += bibliotecaGenJsonString + ", \n";
            }

            resultJson.TrimEnd('\n');
            resultJson = "[" + resultJson.TrimEnd(new char[] { ',', ' ', '\n' }) + "]";
            File.WriteAllText(destinyFilePath, resultJson);
        }

        public static string GetTipo(string tipoOriginal)
        {
            string tipo = tipoOriginal.Split('|')[2] != string.Empty ? tipoOriginal.Split('|')[2] : "Publica";

            if (tiposBibliotecas.ContainsKey(tipo))
            {
                tipo = tiposBibliotecas[tipo];
            }

            return tipo;
        }

        public static string GetTelefono(string telefonoOriginal)
        {
            return telefonoOriginal.Replace(" ", string.Empty);
        }

        public static string GetNombreProvincia(string codMunicipio){

            if (codMunicipio[..2] == "08") return "Barcelona";
            else if (codMunicipio[..2] == "17") return "Girona";
            else if (codMunicipio[..2] == "25") return "Lleida";
            else return "Tarragona";
        }

        public static string GetCodigoProvincia(string codProvincia){

            return codProvincia.Substring(0, 2);
        }

        private static Dictionary<string, string> tiposBibliotecas = new Dictionary<string, string>
        {
            {"Públiques", "Publica"},
            {"Especialitzades", "Especializada"}
        };
    }
}
