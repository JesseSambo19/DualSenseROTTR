using System;
using System.IO;

namespace DualSenseROTTR
{
    public static class DSXPortHelper
    {
        private static readonly string FolderPath = @"C:\Temp\DualSenseX";
        private static readonly string FilePath = @"C:\Temp\DualSenseX\DualSenseX_PortNumber.txt";
        private const string DefaultPort = "6969";

        public static string GetPortNumber()
        {
            try
            {
                // Ensure folder exists
                if (!Directory.Exists(FolderPath))
                {
                    Directory.CreateDirectory(FolderPath);
                    // Console.WriteLine("Folder created");
                }

                // Ensure file exists
                if (!File.Exists(FilePath))
                {
                    File.WriteAllText(FilePath, DefaultPort);
                    // Console.WriteLine("File created");
                }

                // Read file
                string port = File.ReadAllText(FilePath).Trim();

                // Safety validation
                if (string.IsNullOrWhiteSpace(port))
                {
                    Console.WriteLine("Port file empty. Using fallback port.");
                    return DefaultPort;
                }

                return port;
            }
            catch (Exception ex)
            {
                // Hard fallback
                Console.WriteLine("Port file error. Using fallback port 6969.");
                Console.WriteLine(ex.Message);
                string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                string errorMessage = $"[{timestamp}] Unexpected error: {ex.Message}\n{ex.StackTrace}";
                Functions.WriteLog(errorMessage);

                return DefaultPort; // <— fallback path
            }
        }
    }
}