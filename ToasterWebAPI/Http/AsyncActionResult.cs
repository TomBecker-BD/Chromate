using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace Toaster.WebAPI
{
    /// <summary>
    /// Helper class to asynchronously create an HTTP response. 
    /// </summary>
    public class AsyncActionResult : IHttpActionResult
    {
        /// <summary>
        /// Task to get the HTTP response message. 
        /// </summary>
        Task<HttpResponseMessage> _respond;

        /// <summary>
        /// Constructor. 
        /// </summary>
        /// <param name="respond">Task to create the HTTP response. </param>
        public AsyncActionResult(Task<HttpResponseMessage> respond)
        {
            _respond = respond;
        }

        /// <summary>
        /// Return a task to create the HTTP response. 
        /// </summary>
        /// <param name="cancellationToken">Not used. </param>
        /// <returns>Task to create the HTTP response. </returns>
        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            return _respond;
        }
    }

    /// <summary>
    /// Extension methods for creating asynchronous HTTP action results. 
    /// </summary>
    public static class AsyncActionResultHelper
    {
        /// <summary>
        /// Create an async action result from a task to get the response. 
        /// </summary>
        /// <param name="respond">Task to create the HTTP response. </param>
        /// <returns>Async action result. </returns>
        public static AsyncActionResult ToHttpActionResult(this Task<HttpResponseMessage> respond)
        {
            return new AsyncActionResult(respond);
        }

        /// <summary>
        /// Create an async action result from a task to get the response data. 
        /// </summary>
        /// <typeparam name="T">Response data type. </typeparam>
        /// <param name="getData">Task to get the response data. </param>
        /// <param name="request">HTTP request. </param>
        /// <returns>Async action result. </returns>
        public static AsyncActionResult ToJsonOrNotFound<T>(this Task<T> getData, HttpRequestMessage request)
        {
            return getData
                .ContinueWith(data => JsonOrNotFound(data.Result, request), TaskContinuationOptions.NotOnCanceled)
                .ToHttpActionResult();
        }

        /// <summary>
        /// Create an async action result from a task to get the response data. 
        /// </summary>
        /// <typeparam name="T">Response data type. </typeparam>
        /// <param name="getData">Task to get the response data. </param>
        /// <param name="request">HTTP request. </param>
        /// <returns>Async action result. </returns>
        public static AsyncActionResult ToHttpContentResult<T>(this Task<T> getData, HttpRequestMessage request)
        {
            return getData
                .ContinueWith(data => JsonResponse(data.Result, request), TaskContinuationOptions.NotOnCanceled)
                .ToHttpActionResult();
        }

        /// <summary>
        /// Create an async action result from a task that returns an HTTP status code. 
        /// </summary>
        /// <param name="getStatus">Task to execute. </param>
        /// <param name="request">HTTP request. </param>
        /// <returns>Async action result. </returns>
        public static AsyncActionResult ToHttpStatusResult(this Task<HttpStatusCode> getStatus, HttpRequestMessage request)
        {
            return getStatus
                .ContinueWith(status => StatusResponse(status.Result, request), TaskContinuationOptions.NotOnCanceled)
                .ToHttpActionResult();
        }

        /// <summary>
        /// Create an HTTP response with JSON content or a status of NotFound. 
        /// </summary>
        /// <typeparam name="T">Response data type. </typeparam>
        /// <param name="data">Response data. </param>
        /// <param name="request">HTTP request. </param>
        /// <returns></returns>
        static HttpResponseMessage JsonOrNotFound<T>(T data, HttpRequestMessage request)
        {
            return (data == null) ? StatusResponse(HttpStatusCode.NotFound, request) : JsonResponse(data, request);
        }

        /// <summary>
        /// Create an HTTP response with JSON content. 
        /// </summary>
        /// <typeparam name="T">Response data type. </typeparam>
        /// <param name="data">Response data. </param>
        /// <param name="request">HTTP request. </param>
        /// <returns>HTTP response. </returns>
        static HttpResponseMessage JsonResponse<T>(T data, HttpRequestMessage request)
        {
            return new HttpResponseMessage()
            {
                Content = new ObjectContent<T>(data, GlobalConfiguration.Configuration.Formatters.JsonFormatter),
                RequestMessage = request
            };
        }

        /// <summary>
        /// Create an HTTP status response. 
        /// </summary>
        /// <param name="status">Status code. </param>
        /// <param name="request">HTTP request. </param>
        /// <returns>HTTP response. </returns>
        static HttpResponseMessage StatusResponse(HttpStatusCode status, HttpRequestMessage request)
        {
            return new HttpResponseMessage()
            {
                StatusCode = status,
                RequestMessage = request
            };
        }
    }
}