using System.Net.Http;
using System.Threading.Tasks;

namespace Chromate
{
    /// <summary>
    /// Interface for an asychronous HTTP request handler. 
    /// </summary>
    public interface IWebAPI
    {
        /// <summary>
        /// Handle an HTTP request asynchronously. 
        /// </summary>
        /// <param name="request">HTTP request. </param>
        /// <returns>Asynchronous HTTP response. </returns>
        Task<HttpResponseMessage> HandleRequestAsync(HttpRequestMessage request);
    }
}