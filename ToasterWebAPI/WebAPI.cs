using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Dependencies;
using System.Web.Http.Dispatcher;
using System.Web.Http.Hosting;
using Chromate;
using Ninject.Extensions.Logging;

namespace Toaster.WebAPI
{
    /// <summary>
    /// Web API adapter for ASP.NET. Uses ASP.NET for routing and dispatching 
    /// to handle web API requests without an HTTP server. 
    /// </summary>
    public class WebAPI : IDisposable, IWebAPI
    {
        /// <summary>
        /// Configuration. 
        /// </summary>
        HttpConfiguration _config;

        /// <summary>
        /// Controller dispatcher. 
        /// </summary>
        HttpControllerDispatcher _controllerDispatcher;

        /// <summary>
        /// Routing dispatcher. 
        /// </summary>
        HttpRoutingDispatcher _routingDispatcher;

        /// <summary>
        /// Message invoker. 
        /// </summary>
        HttpMessageInvoker _invoker;

        /// <summary>
        /// Logger. 
        /// </summary>
        ILogger _logger;

        /// <summary>
        /// Initialize the web API with a specified dependency resolver. 
        /// </summary>
        /// <param name="resolver">Dependency resolver. </param>
        /// <param name="logger">Logger. </param>
        public WebAPI(IDependencyResolver resolver, ILogger logger)
        {
            _logger = logger;
            _config = new HttpConfiguration();
            WebApiConfig.Register(_config, resolver);
            _config.EnsureInitialized();
            _controllerDispatcher = new HttpControllerDispatcher(_config);
            _routingDispatcher = new HttpRoutingDispatcher(_config, _controllerDispatcher);
            _invoker = new HttpMessageInvoker(_routingDispatcher);
        }

        /// <summary>
        /// Handle an HTTP request asynchronously. 
        /// </summary>
        /// <param name="request">HTTP request. </param>
        /// <returns>Asynchronous HTTP response. </returns>
        public Task<HttpResponseMessage> HandleRequestAsync(HttpRequestMessage request)
        {
#if DEBUG
            //string content = request.Content?.ReadAsStringAsync()?.Result;
            //if (content != null)
            //{
            //    _logger.Trace("{0} {1} content: {2}", request.Method.ToString(), request.RequestUri.ToString(), content);
            //}
            //else
            //{
                _logger.Trace("{0} {1}", request.Method.ToString(), request.RequestUri.ToString());
            //}
#endif
            request.Properties[HttpPropertyKeys.HttpConfigurationKey] = _config;
            CancellationToken cancel = new CancellationToken(false);
            return _invoker.SendAsync(request, cancel);
        }

        #region IDisposable Support

        /// <summary>
        /// Clean up. 
        /// </summary>
        /// <param name="disposing">True if called from Dispose, false if called from finalizer. </param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_invoker != null)
                {
                    _invoker.Dispose();
                    _invoker = null;
                }

                if (_routingDispatcher != null)
                {
                    _routingDispatcher.Dispose();
                    _routingDispatcher = null;
                }

                if (_controllerDispatcher != null)
                {
                    _controllerDispatcher.Dispose();
                    _controllerDispatcher = null;
                }

                if (_config != null)
                {
                    _config.Dispose();
                    _config = null;
                }
            }
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}