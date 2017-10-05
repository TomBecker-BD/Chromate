using System;
using System.Net.Http;
using System.Text;
using CefSharp;

namespace Chromate
{
    /// <summary>
    /// Help convert a CefSharp request to a .NET HTTP request message. 
    /// </summary>
    public static class MessageHelper
    {
        /// <summary>
        /// Convert a CefSharp request to a .NET HTTP request message. 
        /// </summary>
        /// <param name="request">CefSharp request. </param>
        /// <returns>HTTP request message. </returns>
        public static HttpRequestMessage ToHttpRequestMessage(this IRequest request)
        {
            HttpMethod method = GetHttpMethod(request.Method);
            HttpRequestMessage message = new HttpRequestMessage(method, request.Url);
            AddHeaders(message, request);
            AddBody(message, request);
            return message;
        }

        /// <summary>
        /// Add headers from a CefSharp request to a .NET HTTP request message. 
        /// </summary>
        /// <param name="message">HTTP request message. </param>
        /// <param name="request">CefSharp request. </param>
        static void AddHeaders(HttpRequestMessage message, IRequest request)
        {
            if (request.Headers != null)
            {
                foreach (string name in request.Headers.Keys)
                {
                    if (CanAddHeader(name))
                    {
                        string value = request.Headers[name];
                        try
                        {
                            message.Headers.Add(name, value);
                        }
                        catch (Exception ex)
                        {
                            // For developers: If you get this message, maybe the header type needs to be excluded by CanAddHeader. 
                            Console.WriteLine("Could not add header '{0}' because: {1}", name, ex.Message);
                        }
                    }
                }
            }
            if (!string.IsNullOrWhiteSpace(request.ReferrerUrl))
            {
                message.Headers.Referrer = new Uri(request.ReferrerUrl);
            }
        }

        /// <summary>
        /// Determine if a CefSharp header can be added to a .NET HTTP request message. 
        /// </summary>
        /// <remarks>
        /// The "content-type" header cannot be added to a .NET HTTP request message 
        /// because .NET stores the content in a separate object. 
        /// </remarks>
        /// <param name="name">Header name. </param>
        /// <returns>True if the header can be added to a .NET HTTP request message, false if not. </returns>
        static bool CanAddHeader(string name)
        {
            return !string.Equals(name, "content-type", StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Add the body from a CefSharp request to a .NET HTTP request message. 
        /// </summary>
        /// <param name="message">HTTP request message. </param>
        /// <param name="request">CefSharp request. </param>
        static void AddBody(HttpRequestMessage message, IRequest request)
        {
            if ((message.Method == HttpMethod.Post) || (message.Method == HttpMethod.Put))
            {
                if (request.PostData != null)
                {
                    if (request.PostData.Elements != null && request.PostData.Elements.Count > 0)
                    {
                        string content = Encoding.UTF8.GetString(request.PostData.Elements[0].Bytes);
                        message.Content = new StringContent(content, Encoding.UTF8, "application/json");
                    }
                }
            }
        }

        /// <summary>
        /// Get the .NET HTTP method for a CefSharp request method name. 
        /// </summary>
        /// <param name="methodName">CefSharp request method name. </param>
        /// <returns>HTTP method object. </returns>
        public static HttpMethod GetHttpMethod(string methodName)
        {
            HttpMethod method = null;
            switch (methodName)
            {
                case "GET":
                    method = HttpMethod.Get;
                    break;
                case "POST":
                    method = HttpMethod.Post;
                    break;
                case "PUT":
                    method = HttpMethod.Put;
                    break;
                case "DELETE":
                    method = HttpMethod.Delete;
                    break;
                default:
                    throw new ArgumentOutOfRangeException("methodName");
            }
            return method;
        }
    }
}
