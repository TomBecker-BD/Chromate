using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http.Dependencies;
using Ninject;
using Ninject.Syntax;

namespace Toaster.WebAPI
{
    /// <summary>
    /// Simple Ninject dependency resolver for use with ASP.Net. 
    /// </summary>
    public class SimpleNinjectDependencyResolver : IDependencyResolver
    {
        /// <summary>
        /// Ninject resolver. 
        /// </summary>
        IResolutionRoot _resolver;

        /// <summary>
        /// Initialize the simple Ninject dependency resolver. 
        /// </summary>
        /// <param name="resolver">Ninject resolver. </param>
        public SimpleNinjectDependencyResolver(IResolutionRoot resolver)
        {
            _resolver = resolver;
        }

        /// <summary>
        /// Starts a resolution scope.
        /// </summary>
        /// <returns>The dependency scope.</returns>
        public IDependencyScope BeginScope()
        {
            return this;
        }

        /// <summary>
        /// Retrieves a service from the scope.
        /// </summary>
        /// <param name="serviceType">The service to be retrieved.</param>
        /// <returns>The retrieved service.</returns>
        public object GetService(Type serviceType)
        {
            object service = _resolver.TryGet(serviceType);
            return service;
        }

        /// <summary>
        /// Retrieves a collection of services from the scope.
        /// </summary>
        /// <param name="serviceType">The collection of services to be retrieved.</param>
        /// <returns>The retrieved collection of services.</returns>
        public IEnumerable<object> GetServices(Type serviceType)
        {
            // This API is called by ASP.NET but not for anything we need to resolve. 
            return Enumerable.Empty<object>();
        }

        /// <summary>
        /// Clean up. 
        /// </summary>
        public void Dispose()
        {
        }
    }
}