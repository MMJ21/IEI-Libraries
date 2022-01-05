using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Extractor
{
    class ExecuteExtractor
    {
        public static void Main(string[] args)
        {
            DatabaseHook.DropTableFromDatabase();

            DatabaseHook.CreateTableInDatabase();

            WrapperEUS.ExtractEUS(
                "C:\\Users\\Administrador.WIN-2O4P6U7CI32\\source\\repos\\Asefron29\\ExtractorIEI\\EntradasDemo\\EUS.json",
                "C:\\Users\\Administrador.WIN-2O4P6U7CI32\\source\\repos\\Asefron29\\ExtractorIEI\\SalidasDemo\\ExtractedEUS.json");
            
            WrapperCAT.ExtractCAT(
                "C:\\Users\\Administrador.WIN-2O4P6U7CI32\\source\\repos\\Asefron29\\ExtractorIEI\\EntradasDemo\\CAT.xml",
                "C:\\Users\\Administrador.WIN-2O4P6U7CI32\\source\\repos\\Asefron29\\ExtractorIEI\\SalidasDemo\\ExtractedCAT.json");
            
            WrapperCV.ExtractCV(
                "C:\\Users\\Administrador.WIN-2O4P6U7CI32\\source\\repos\\Asefron29\\ExtractorIEI\\EntradasDemo\\CV.csv",
                "C:\\Users\\Administrador.WIN-2O4P6U7CI32\\source\\repos\\Asefron29\\ExtractorIEI\\EntradasDemo\\bibliotecasCV.json",
                "C:\\Users\\Administrador.WIN-2O4P6U7CI32\\source\\repos\\Asefron29\\ExtractorIEI\\SalidasDemo\\ExtractedCV.json");
            
            DatabaseHook.SendToDatabase(
                "C:\\Users\\Administrador.WIN-2O4P6U7CI32\\source\\repos\\Asefron29\\ExtractorIEI\\SalidasDemo\\ExtractedEUS.json",
                "C:\\Users\\Administrador.WIN-2O4P6U7CI32\\source\\repos\\Asefron29\\ExtractorIEI\\SalidasDemo\\ExtractedCAT.json",
                "C:\\Users\\Administrador.WIN-2O4P6U7CI32\\source\\repos\\Asefron29\\ExtractorIEI\\SalidasDemo\\ExtractedCV.json");

            Thread.Sleep(2000);

            Environment.Exit(0);
        }
    }
}
