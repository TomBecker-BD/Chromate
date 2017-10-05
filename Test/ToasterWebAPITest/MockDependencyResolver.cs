using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http.Dependencies;
using System.Web.Http.ExceptionHandling;
using Ninject.Extensions.Logging;
using Toaster.Interfaces;
using Toaster.WebAPI.Controllers;

namespace Toaster.WebAPI.Test
{
    public class MockDependencyResolver : IDependencyResolver
    {
        IToaster _toaster;
        IToasterStatusMonitor _statusMonitor;
        ILogger _logger;

        public MockDependencyResolver(IToaster toaster, IToasterStatusMonitor statusMonitor, ILogger logger)
        {
            _toaster = toaster;
            _statusMonitor = statusMonitor;
            _logger = logger;
        }

        public IDependencyScope BeginScope()
        {
            return this;
        }

        public void Dispose()
        {
        }

        public object GetService(Type serviceType)
        {
            object service = null;
            if (serviceType == typeof(ToasterController))
            {
                service = new ToasterController(_toaster, _statusMonitor);
            }
            else if (serviceType == typeof(IExceptionLogger))
            {
                service = new UnhandledExceptionLogger(_logger);
            }
            return service;
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return Enumerable.Empty<object>();
        }
    }
}