using System;
using Ninject.Extensions.Logging;

namespace TestingUtilities
{
    public class ConsoleLogger : ILogger
    {
        public InfoLevel CurrentLevel { get; set; }

        public ConsoleLogger ()
        {
            CurrentLevel = InfoLevel.Trace;
        }

        public bool IsDebugEnabled
        {
            get
            {
                return (CurrentLevel <= InfoLevel.Debug);
            }
        }

        public bool IsErrorEnabled
        {
            get
            {
                return (CurrentLevel <= InfoLevel.Error);
            }
        }

        public bool IsFatalEnabled
        {
            get
            {
                return (CurrentLevel <= InfoLevel.Fatal);
            }
        }

        public bool IsInfoEnabled
        {
            get
            {
                return (CurrentLevel <= InfoLevel.Info);
            }
        }

        public bool IsTraceEnabled
        {
            get
            {
                return (CurrentLevel <= InfoLevel.Trace);
            }
        }

        public bool IsWarnEnabled
        {
            get
            {
                return (CurrentLevel <= InfoLevel.Warn);
            }
        }

        public string Name
        {
            get
            {
                return "UnitTest";
            }
        }

        public Type Type
        {
            get
            {
                return typeof(ConsoleLogger);
            }
        }

        public void Debug(string message)
        {
            if (IsDebugEnabled)
            {
                Console.WriteLine("Debug|" + message);
            }
        }

        public void Debug(string format, params object[] args)
        {
            if (IsDebugEnabled)
            {
                Console.WriteLine("Debug|" + format, args);
            }
        }

        public void Debug(Exception exception, string format, params object[] args)
        {
            if (IsDebugEnabled)
            {
                Console.WriteLine("Debug|" + format, args);
                Console.WriteLine(exception.ToString());
            }
        }

        public void DebugException(string message, Exception exception)
        {
            if (IsDebugEnabled)
            {
                Console.WriteLine("Debug|" + message);
                Console.WriteLine(exception.ToString());
            }
        }

        public void Error(string message)
        {
            if (IsErrorEnabled)
            {
                Console.WriteLine("Error|" + message);
            }
        }

        public void Error(string format, params object[] args)
        {
            if (IsErrorEnabled)
            {
                Console.WriteLine("Error|" + format, args);
            }
        }

        public void Error(Exception exception, string format, params object[] args)
        {
            if (IsErrorEnabled)
            {
                Console.WriteLine("Error|" + format, args);
                Console.WriteLine(exception.ToString());
            }
        }

        public void ErrorException(string message, Exception exception)
        {
            if (IsErrorEnabled)
            {
                Console.WriteLine("Error|" + message);
                Console.WriteLine(exception.ToString());
            }
        }

        public void Fatal(string message)
        {
            if (IsFatalEnabled)
            {
                Console.WriteLine("Fatal|" + message);
            }
        }

        public void Fatal(string format, params object[] args)
        {
            if (IsFatalEnabled)
            {
                Console.WriteLine("Fatal|" + format, args);
            }
        }

        public void Fatal(Exception exception, string format, params object[] args)
        {
            if (IsFatalEnabled)
            {
                Console.WriteLine("Fatal|" + format, args);
                Console.WriteLine(exception.ToString());
            }
        }

        public void FatalException(string message, Exception exception)
        {
            if (IsFatalEnabled)
            {
                Console.WriteLine("Fatal|" + message);
                Console.WriteLine(exception.ToString());
            }
        }

        public void Info(string message)
        {
            if (IsInfoEnabled)
            {
                Console.WriteLine("Info|" + message);
            }
        }

        public void Info(string format, params object[] args)
        {
            if (IsInfoEnabled)
            {
                Console.WriteLine("Info|" + format, args);
            }
        }

        public void Info(Exception exception, string format, params object[] args)
        {
            if (IsInfoEnabled)
            {
                Console.WriteLine("Info|" + format, args);
                Console.WriteLine(exception.ToString());
            }
        }

        public void InfoException(string message, Exception exception)
        {
            if (IsInfoEnabled)
            {
                Console.WriteLine("Info|" + message);
                Console.WriteLine(exception.ToString());
            }
        }

        public void Trace(string message)
        {
            if (IsTraceEnabled)
            {
                Console.WriteLine("Trace|" + message);
            }
        }

        public void Trace(string format, params object[] args)
        {
            if (IsTraceEnabled)
            {
                Console.WriteLine("Trace|" + format, args);
            }
        }

        public void Trace(Exception exception, string format, params object[] args)
        {
            if (IsTraceEnabled)
            {
                Console.WriteLine("Trace|" + format, args);
                Console.WriteLine(exception.ToString());
            }
        }

        public void TraceException(string message, Exception exception)
        {
            if (IsTraceEnabled)
            {
                Console.WriteLine("Trace|" + message);
                Console.WriteLine(exception.ToString());
            }
        }

        public void Warn(string message)
        {
            if (IsWarnEnabled)
            {
                Console.WriteLine("Warn|" + message);
            }
        }

        public void Warn(string format, params object[] args)
        {
            if (IsWarnEnabled)
            {
                Console.WriteLine("Warn|" + format, args);
            }
        }

        public void Warn(Exception exception, string format, params object[] args)
        {
            if (IsWarnEnabled)
            {
                Console.WriteLine("Warn|" + format, args);
                Console.WriteLine(exception.ToString());
            }
        }

        public void WarnException(string message, Exception exception)
        {
            if (IsWarnEnabled)
            {
                Console.WriteLine("Warn|" + message);
                Console.WriteLine(exception.ToString());
            }
        }
    }
}
