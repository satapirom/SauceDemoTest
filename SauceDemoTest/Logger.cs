using System;
using System.IO;

namespace SauceDemoTest.Utils
{
    public class Logger : IDisposable
    {
        private StreamWriter? logFile;

        public Logger(string fileName = "test_log.txt")
        {
            // หาตำแหน่ง root project (ขึ้นไป 3 ระดับจาก bin/Debug/netX.0/)
            string projectPath = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\.."));
            string logPath = Path.Combine(projectPath, fileName);

            logFile = new StreamWriter(logPath, append: true);
            Console.WriteLine($"Log file created at: {logPath}");
        }

        public void Log(string action, string status = "INFO")
        {
            string text = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} [{status}] {action}";
            Console.WriteLine(text);
            logFile?.WriteLine(text);
            logFile?.Flush();
        }

        public void Dispose()
        {
            logFile?.Close();
            logFile?.Dispose();
        }
    }
}
