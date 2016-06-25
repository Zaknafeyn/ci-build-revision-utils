using System;

namespace AndroidManifestUtil.Services
{
    public class ConsoleLogger : ILogger
    {
        public void Log(string msg, LogSeverity logSeverity = LogSeverity.Info)
        {
            Console.ForegroundColor = GetColorBySeverity(logSeverity);

            Console.WriteLine(msg);

            Console.ResetColor();
        }

        private ConsoleColor GetColorBySeverity(LogSeverity severity)
        {
            switch (severity)
            {
                case LogSeverity.Debug:
                    return ConsoleColor.Gray;
                case LogSeverity.Info:
                    return ConsoleColor.Green;
                case LogSeverity.Warning:
                    return ConsoleColor.DarkYellow;
                case LogSeverity.Error:
                    return ConsoleColor.Yellow;
                case LogSeverity.Fatal:
                    return ConsoleColor.Red;
                default:
                    throw new ArgumentOutOfRangeException(nameof(severity), severity, null);
            }
        }

    }
}