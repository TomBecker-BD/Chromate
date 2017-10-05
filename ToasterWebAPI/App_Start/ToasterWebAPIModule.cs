using System.Web.Http.Dependencies;
using System.Web.Http.ExceptionHandling;
using Chromate;
using Ninject.Modules;

namespace Toaster.WebAPI
{
    /// <summary>
    /// Define bindings for the toaster web API module. 
    /// </summary>
    public class ToasterWebAPIModule : NinjectModule
    {
        /// <summary>
        /// Loads the module into the kernel.
        /// </summary>
        public override void Load()
        {
            Bind<IDependencyResolver>().To<SimpleNinjectDependencyResolver>();
            Bind<IWebAPI>().To<WebAPI>();
            Bind<IExceptionLogger>().To<UnhandledExceptionLogger>();
            Bind<IToasterStatusMonitor>().To<ToasterStatusMonitor>().InSingletonScope();
        }
    }
}