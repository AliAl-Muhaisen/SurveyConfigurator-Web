using System;
using System.IO;
using System.Reflection;
using System.Web;

namespace SurveyConfiguratorApp.Helper
{
    public class Log
    {
        private static readonly string appLocation = HttpRuntime.AppDomainAppVirtualPath != null ?
    Path.Combine(HttpRuntime.AppDomainAppPath, "bin") : Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
        private static object lockObject = new object();

        private static string CreateFile(string fileName)
        {
            try
            {
                string tLogDirectory = Path.Combine(appLocation, "logs");
                Directory.CreateDirectory(tLogDirectory);

                string logFilePath = Path.Combine(tLogDirectory, $"{fileName}.txt");
                return logFilePath;
            }
            catch (Exception e)
            {

                throw;
            }
        }

        static public void Error(Exception pEx)
        {
            try
            {

                string tLogFilePath = CreateFile("Error.Log");

                // to ensure that only one thread at a time can write to the log file
                lock (lockObject)
                {
                    using (StreamWriter tStreamWriter = new StreamWriter(tLogFilePath, true))
                    {
                        tStreamWriter.WriteLine("-----   Error   -----");
                        tStreamWriter.WriteLine($"DateTime: {DateTime.Now}");
                        tStreamWriter.WriteLine($"Type: {pEx.GetType()}");
                        tStreamWriter.WriteLine($"Message: {pEx.Message}");
                        tStreamWriter.WriteLine($"Stack Trace: {pEx.StackTrace}");
                        tStreamWriter.WriteLine("\n\n");
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error handling exception: {e.Message}");
            }

        }

        static public void Info(string pInfo)
        {
            try
            {


                string logFilePath = CreateFile("Error.Log");

                // to ensure that only one thread at a time can write to the log file
                lock (lockObject)
                {
                    using (StreamWriter streamWriter = new StreamWriter(logFilePath, true))
                    {
                        streamWriter.WriteLine("-----   Info   -----");
                        streamWriter.WriteLine($"DateTime: {DateTime.Now}");
                        streamWriter.WriteLine($"Message: {pInfo}");

                        streamWriter.WriteLine("\n\n");
                    }
                }
            }
            catch (Exception handleEx)
            {
                Console.WriteLine($"Error handling exception: {handleEx.Message}");
            }

        }
    }

}
