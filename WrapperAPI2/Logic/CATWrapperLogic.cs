using CATWrapper.Models;
using System.Text.Json;
using System.Xml;

namespace CATWrapper.Logic
{
    public static class CATWrapperLogic
    {
        public static List<BibliotecaGEN> GetBibliotecas()
        {
            XmlDocument document = new XmlDocument();
            document.Load(".\\Resources\\CATDemo.xml");
            XmlNode responseNode = document.DocumentElement;
            List<BibliotecaGEN> myBibliotecas = new List<BibliotecaGEN>();
            BibliotecaGEN bibliotecaGEN;

            foreach (XmlNode node in responseNode.ChildNodes)
            {
                bibliotecaGEN = new BibliotecaGEN();
                bibliotecaGEN.nombre = (node.SelectSingleNode("nom") != null ? node.SelectSingleNode("nom").InnerText : "Not found").Replace("\'", "´");
                bibliotecaGEN.tipo = (node.SelectSingleNode("categoria") != null ? GetTipo(node.SelectSingleNode("categoria").InnerText) : "Not found").Replace("\'", "´");
                bibliotecaGEN.direccion = (node.SelectSingleNode("via") != null ? node.SelectSingleNode("via").InnerText : "Not found").Replace("\'", "´");
                bibliotecaGEN.codigoPostal = (node.SelectSingleNode("cpostal") != null ? node.SelectSingleNode("cpostal").InnerText : "Not found").Replace("\'", "´");
                bibliotecaGEN.longitud = (node.SelectSingleNode("longitud") != null ? node.SelectSingleNode("longitud").InnerText : "Not found").Replace("\'", "´");
                bibliotecaGEN.latitud = (node.SelectSingleNode("latitud") != null ? node.SelectSingleNode("latitud").InnerText : "Not found").Replace("\'", "´");
                bibliotecaGEN.telefono = (node.SelectSingleNode("telefon1") != null ? GetTelefono(node.SelectSingleNode("telefon1").InnerText) : "Not found").Replace("\'", "´");
                bibliotecaGEN.email = (node.SelectSingleNode("email") != null ? node.SelectSingleNode("email").InnerText : "Not found").Replace("\'", "´");
                bibliotecaGEN.descripcion = (node.SelectSingleNode("propietats") != null ? node.SelectSingleNode("propietats").InnerText : "Not found").Replace("\'", "´");
                bibliotecaGEN.nombreLocalidad = (node.SelectSingleNode("poblacio") != null ? node.SelectSingleNode("poblacio").InnerText : "Not found").Replace("\'", "´");
                bibliotecaGEN.codigoLocalidad = ((node.SelectSingleNode("codi_municipi") != null ? node.SelectSingleNode("codi_municipi").InnerText : "Not found").Replace("\'", "´"))[2..];
                bibliotecaGEN.nombreProvincia = GetNombreProvincia(bibliotecaGEN.codigoLocalidad);
                bibliotecaGEN.codigoProvincia = GetCodigoProvincia(node.SelectSingleNode("codi_municipi") != null ? node.SelectSingleNode("codi_municipi").InnerText : "Not found");

                myBibliotecas.Add(bibliotecaGEN);
            }

            return myBibliotecas;
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

        public static string GetNombreProvincia(string codMunicipio)
        {

            if (codMunicipio[..2] == "08") return "Barcelona";
            else if (codMunicipio[..2] == "17") return "Girona";
            else if (codMunicipio[..2] == "25") return "Lleida";
            else return "Tarragona";
        }

        public static string GetCodigoProvincia(string codProvincia)
        {

            return codProvincia.Substring(0, 2);
        }

        private static Dictionary<string, string> tiposBibliotecas = new Dictionary<string, string>
        {
            {"Públiques", "Publica"},
            {"Especialitzades", "Especializada"}
        };
    }
}
