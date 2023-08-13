using System;
using System.IO;

namespace JCorePanel
{
    public static class Logger
    {
        private static string logFilePath;
        private const string LogFileName = "latest.log";

        static Logger()
        {
            string logDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs");
            string fullLogFilePath = Path.Combine(logDirectory, LogFileName);
            EnsureLogDirectoryExists(logDirectory);
            CreateNewLogFile(fullLogFilePath);
            logFilePath = fullLogFilePath;
        }

        private static void EnsureLogDirectoryExists(string logDirectory)
        {
            if (!Directory.Exists(logDirectory))
            {
                Directory.CreateDirectory(logDirectory);
            }
        }

        private static void CreateNewLogFile(string fullLogFilePath)
        {
            if (File.Exists(fullLogFilePath))
            {
                DateTime creationTime = File.GetLastWriteTime(fullLogFilePath);
                string timestamp = creationTime.ToString("yyyyMMdd_HHmmss");
                string newLogFileName = $"{timestamp}.log";
                string newLogFilePath = Path.Combine(Path.GetDirectoryName(fullLogFilePath), newLogFileName);

                File.Move(fullLogFilePath, newLogFilePath);
            }
            else
            {
                File.Create(fullLogFilePath).Dispose();
            }
        }


        public static void Log(LogLevel level, string message)
        {
            string logEntry = $"[{level}][{DateTime.Now}] {message}";

            try
            {
                using (StreamWriter writer = new StreamWriter(logFilePath, true))
                {
                    writer.WriteLine(logEntry);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error writing to log file: {ex.Message}");
            }
        }
        public static void Log(string message)
        {
            string logEntry = $"[{LogLevel.Info}][{DateTime.Now}] {message}";

            try
            {
                using (StreamWriter writer = new StreamWriter(logFilePath, true))
                {
                    writer.WriteLine(logEntry);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error writing to log file: {ex.Message}");
            }
        }
    }

    public enum LogLevel
    {
        Info,
        Warning,
        Error
    }
}
