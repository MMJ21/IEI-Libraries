using System.Text.Json;
using System.IO;
using System.Collections.Generic;
using System;
using System.Xml;
using System.Web;
using ChoETL;
using System.Text;
using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Text.RegularExpressions;

namespace Extractor
{
    public class WrapperCV
    {
        static readonly ChromeDriver driver = new ChromeDriver();        

        public static void ExtractCV(string originFilePath, string jsonDestinyFilePath, string destinyFilePath)
        {
            Console.WriteLine("::EXTRACTING VAL LIBRARIES::");
            string csv = File.ReadAllText(originFilePath);

            StringBuilder sb = new StringBuilder();
            using (var p = ChoCSVReader.LoadText(csv)
                .WithFirstLineHeader()
                )
            {
                using (var w = new ChoJSONWriter(sb))
                    w.Write(p);
            }
            string outString = sb.ToString();
            File.WriteAllText(jsonDestinyFilePath, outString);

            string jsonString = File.ReadAllText(jsonDestinyFilePath);
            string resultJson = string.Empty;
            jsonString = jsonString.Replace("[]", '\u0022'.ToString() + '\u0022'.ToString());
            List<BibliotecaCV> bibliotecasCV = JsonSerializer.Deserialize<List<BibliotecaCV>>(jsonString);
            BibliotecaGEN bibliotecaGEN = new BibliotecaGEN();
            string bibliotecaGenJsonString = string.Empty;

            Console.WriteLine(":USING SELENIUM TO GET COORDINATES FOR VAL LIBRARIES, THIS MIGHT TAKE A WHILE:");
            Thread.Sleep(5000);

            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
            };

            foreach (BibliotecaCV biblioteca in bibliotecasCV)
            {
                bibliotecaGEN.nombre = GetNombre(biblioteca.NOMBRE, biblioteca.TIPO); //hecho
                bibliotecaGEN.tipo = GetTipo(Descapitalizar(biblioteca.TIPO)); //hecho
                bibliotecaGEN.direccion = GetDireccion(biblioteca.DIRECCION.Replace("?", "ñ")); //hecho
                bibliotecaGEN.codigoPostal = biblioteca.CP; //hecho
                bibliotecaGEN.telefono = biblioteca.TELEFONO != string.Empty ? Regex.Replace(biblioteca.TELEFONO.Substring(5), "[A-Za-z ]", "") : string.Empty; //hecho
                bibliotecaGEN.email = biblioteca.EMAIL.ToLower(); //hecho
                bibliotecaGEN.descripcion = "Descripcion Generica";
                bibliotecaGEN.nombreLocalidad = Descapitalizar(biblioteca.NOM_MUNICIPIO); //hecho
                bibliotecaGEN.codigoLocalidad = biblioteca.COD_MUNICIPIO; //hecho
                bibliotecaGEN.nombreProvincia = Descapitalizar(biblioteca.NOM_PROVINCIA); //hecho
                bibliotecaGEN.codigoProvincia = biblioteca.COD_PROVINCIA; //hecho
                try
                {
                    /*
                    driver.FindElement(By.Id("address")).Clear();
                    Thread.Sleep(1000);
                    driver.FindElement(By.Id("address")).SendKeys(bibliotecaGEN.direccion.Split("Nº")[0] + ", " + bibliotecaGEN.codigoPostal + ", España");
                    Thread.Sleep(1000);
                    driver.FindElement(By.XPath("//button[text()= " + '\u0022'.ToString() + "Obtener Coordenadas GPS" + '\u0022'.ToString() + "]")).Click();
                    Thread.Sleep(1000);

                    
                    bibliotecaGEN.longitud = driver.FindElement(By.Id("latitude")).GetAttribute("value");
                    bibliotecaGEN.latitud = driver.FindElement(By.Id("longitude")).GetAttribute("value");
                    */

                    string[] coordenadas = buscarCoordenadasGPS(bibliotecaGEN);
                    bibliotecaGEN.longitud = coordenadas[0];
                    bibliotecaGEN.latitud = coordenadas[1];
                }
                catch (UnhandledAlertException e)
                {
                    bibliotecaGEN.longitud = "0";
                    bibliotecaGEN.latitud = "0";
                }

                bibliotecaGenJsonString = JsonSerializer.Serialize(bibliotecaGEN, options);
                resultJson += bibliotecaGenJsonString + ", \n";
            }

            driver.Close();

            resultJson.TrimEnd('\n');
            resultJson = "[" + resultJson.TrimEnd(new char[] { ',', ' ',  '\n'}) + "]";
            File.WriteAllText(destinyFilePath, resultJson);
        }

        private static string[] buscarCoordenadasGPS(BibliotecaGEN bibliotecaGEN)
        {
            driver.Url = "https://maps.google.com";
            driver.ExecuteScript("window.scrollBy(0,1000)");

            driver.FindElement(By.XPath("/html/body/c-wiz/div/div/div/div[2]/div/div[4]/form/div"))?.Click();
            Thread.Sleep(1000);
            driver.FindElement(By.XPath("html/body/div[3]/div[9]/div[2]/div/div/div/div[2]/form/")).SendKeys(bibliotecaGEN.direccion.Split("Nº")[0] + ", " + bibliotecaGEN.codigoPostal + ", España" + Keys.Enter);

            string[] coordenadas = new string[2];
            coordenadas[0] = driver.Url.Split('@')[1].Split(',')[0];
            coordenadas[1] = driver.Url.Split('@')[1].Split(',')[1].Split('/')[0];

            return coordenadas;
        }

        public static string Descapitalizar(string srt)
        {
           string res;
           try
            {
                res = srt.Substring(0,1).ToUpper() + srt.Substring(1).ToLower();
            } 
            catch {return string.Empty; }
            return res;
        }

        public static string GetTipo(string tipoOriginal)
        {
            if(tiposBibliotecas.ContainsKey(tipoOriginal))
            {
                return tiposBibliotecas[tipoOriginal];
            }
            else
            {
                return "Publica";
            }
        }

         public static string GetNombre(string nombreOriginal, string tipo)
        {
            return Descapitalizar(nombreOriginal.Replace(tipo, string.Empty).TrimStart());
        }

        public static string GetDireccion(string direccionOriginal)
        {
            string res = Descapitalizar(direccionOriginal);
            return res.Replace(" nº", ",");
        }

        private static Dictionary<string, string> tiposBibliotecas = new Dictionary<string, string>
        {
            {"Biblioteca publica municipal", "Publica"},
            {"Agencia de lectura", "Publica"},
            {"Biblioteca infantil", "Publica" },
            {"Biblioteca especializada", "Especializada"}
        };
    }
}