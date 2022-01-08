using CVWrapper.Models;
using System.Text;
using System.Text.Json;
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

namespace CVWrapper.Logic
{
    public static class CVWrapperLogic
    {
        public static List<BibliotecaGEN> GetBibliotecas()
        {
            string csv = File.ReadAllText(".\\Resources\\CVDemo.csv");

            StringBuilder sb = new StringBuilder();
            using (var p = ChoCSVReader.LoadText(csv)
                .WithFirstLineHeader()
                )
            {
                using (var w = new ChoJSONWriter(sb))
                    w.Write(p);
            }
            string outString = sb.ToString();

            string jsonString = outString;
            jsonString = jsonString.Replace("[]", '\u0022'.ToString() + '\u0022'.ToString());
            List<BibliotecaCV> bibliotecasCV = JsonSerializer.Deserialize<List<BibliotecaCV>>(jsonString);
            BibliotecaGEN bibliotecaGEN = new BibliotecaGEN();
            List<BibliotecaGEN> myBibliotecas = new List<BibliotecaGEN>();

            Console.WriteLine(":USING SELENIUM TO GET COORDINATES FOR VAL LIBRARIES, THIS MIGHT TAKE A WHILE:");
            ChromeDriver driver = new ChromeDriver();
            driver.Url = "https://www.coordenadas-gps.com/";
            Thread.Sleep(5000);

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
                    driver.FindElement(By.Id("address")).Clear();
                    Thread.Sleep(1000);
                    driver.FindElement(By.Id("address")).SendKeys(bibliotecaGEN.direccion.Split("Nº")[0] + ", " + bibliotecaGEN.codigoPostal + ", España");
                    Thread.Sleep(1000);
                    driver.FindElement(By.XPath("//button[text()= " + '\u0022'.ToString() + "Obtener Coordenadas GPS" + '\u0022'.ToString() + "]")).Click();
                    Thread.Sleep(1000);

                    bibliotecaGEN.longitud = driver.FindElement(By.Id("latitude")).GetAttribute("value");
                    bibliotecaGEN.latitud = driver.FindElement(By.Id("longitude")).GetAttribute("value");
                }
                catch (UnhandledAlertException e)
                {
                    bibliotecaGEN.longitud = "0";
                    bibliotecaGEN.latitud = "0";
                }

                myBibliotecas.Add(bibliotecaGEN);
            }

            driver.Close();

            return myBibliotecas;
        }

        public static string Descapitalizar(string srt)
        {
            string res;
            try
            {
                res = srt.Substring(0, 1).ToUpper() + srt.Substring(1).ToLower();
            }
            catch { return string.Empty; }
            return res;
        }

        public static string GetTipo(string tipoOriginal)
        {
            if (tiposBibliotecas.ContainsKey(tipoOriginal))
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
