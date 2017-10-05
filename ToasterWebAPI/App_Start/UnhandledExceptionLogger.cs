using System.Web.Http.ExceptionHandling;
using Ninject.Extensions.Logging;

namespace Toaster.WebAPI
{
    public class UnhandledExceptionLogger : ExceptionLogger
    {
        ILogger _logger;

        public UnhandledExceptionLogger(ILogger logger)
            : base()
        {
            _logger = logger;
        }

        public override void Log(ExceptionLoggerContext context)
        {
            _logger.WarnException(string.Format("{0} {1}", context.Request.Method.ToString(), context.Request.RequestUri.OriginalString), context.Exception);
        }
    }
}