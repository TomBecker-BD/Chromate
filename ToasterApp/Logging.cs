using System;
using NLog;
using NLog.Config;
using NLog.Targets;

namespace Toaster.App
{
    /// <summary>
    /// Configure logging in code. 
    /// </summary>
    public static class Logging
    {
        /// <summary>
        /// Configure logging. 
        /// </summary>
        /// <param name="level">Minimum log level to enable. </param>
        /// <param name="filename">Log filename. </param>
        public static void Configure(LogLevel level, string filename)
        {
            var config = new LoggingConfiguration();
            var consoleTarget = new ConsoleTarget("console")
            {
                DetectConsoleAvailable = true,
                Layout = "${message} ${exception}",
            };
            //var bufferConsole = new BufferingTargetWrapper("console-buffered", consoleTarget)
            //{
            //    BufferSize = 10,
            //    FlushTimeout = 100,
            //};
            config.AddTarget(consoleTarget);
            config.LoggingRules.Add(new LoggingRule("*", level, consoleTarget));

            if (!string.IsNullOrEmpty(filename))
            {
                var fileTarget = new FileTarget("file")
                {
                    FileName = filename,
                    Layout = "${longdate}|${level:uppercase=true}|${logger}|${message} ${exception:format=ToString}",
                };
                config.AddTarget(fileTarget);
                config.LoggingRules.Add(new LoggingRule("*", level, fileTarget));
            }

            LogManager.Configuration = config;
        }

        /// <summary>
        /// Change the enabled log level. 
        /// </summary>
        /// <param name="levelName">Log level to enable. </param>
        public static void EnableLoggingForLevel(string levelName)
        {
            if (string.IsNullOrEmpty(levelName))
            {
                levelName = "Info";
            }
            LogLevel level = LogLevel.FromString(levelName);
            EnableLoggingForLevel(level);
        }

        /// <summary>
        /// Change the enabled log level. 
        /// </summary>
        /// <param name="level">Log level to enable. </param>
        public static void EnableLoggingForLevel(LogLevel level)
        {
            foreach (var rule in LogManager.Configuration.LoggingRules)
            {
                rule.EnableLoggingForLevels(level, LogLevel.Fatal);
            }
            LogManager.ReconfigExistingLoggers();
        }
    }
}
