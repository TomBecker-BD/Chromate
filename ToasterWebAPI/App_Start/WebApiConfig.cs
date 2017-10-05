using System.Web.Http;
using System.Web.Http.Dependencies;
using System.Web.Http.ExceptionHandling;

namespace Toaster.WebAPI
{
    public static class WebApiConfig
    {
        /// <summary>
        /// Set up the configuration using a specified dependency resolver. 
        /// </summary>
        /// <param name="config">Configuration. </param>
        /// <param name="resolver">Dependency resolver. </param>
        public static void Register(HttpConfiguration config, IDependencyResolver resolver)
        {
            config.DependencyResolver = resolver;
            config.Services.Replace(typeof(IExceptionLogger), resolver.GetService(typeof(IExceptionLogger)));

            config.MapHttpAttributeRoutes();
        }
    }
}
