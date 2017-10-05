using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using CefSharp;

namespace Chromate
{
    /// <summary>
    /// Custom CefSharp resource handler that delegates to an ASP.NET web API handler. 
    /// </summary>
    public class ApiResourceHandler : IResourceHandler
    {
        /// <summary>
        /// Web API handler. 
        /// </summary>
        IWebAPI _webAPI;

        /// <summary>
        /// ASP.NET HTTP response message object. 
        /// </summary>
        HttpResponseMessage _responseMessage;

        /// <summary>
        /// Serialized HTTP response content. 
        /// </summary>
        byte[] _responseContent;

        /// <summary>
        /// Current read offset in the reponse content.
        /// </summary>
        int _reponseOffset;

        /// <summary>
        /// Initialize the API resource handler. 
        /// </summary>
        /// <param name="webAPI">Web API handler. </param>
        public ApiResourceHandler(IWebAPI webAPI)
        {
            _webAPI = webAPI;
        }

        /// <summary>
        /// Request processing has been canceled.
        /// </summary>
        public void Cancel()
        {
        }

        /// <summary>
        /// Return true if the specified cookie can be sent with the request or false otherwise. 
        /// If false is returned for any cookie then no cookies will be sent with the request. 
        /// </summary>
        /// <param name="cookie">Cookie</param>
        /// <returns>True</returns>
        public bool CanGetCookie(Cookie cookie)
        {
            return true;
        }

        /// <summary>
        /// Return true if the specified cookie returned with the response can be set or 
        /// false otherwise.
        /// </summary>
        /// <param name="cookie">Cookie</param>
        /// <returns>True</returns>
        public bool CanSetCookie(Cookie cookie)
        {
            return true;
        }

        /// <summary>
        /// Begin processing the request.
        /// </summary>
        /// <param name="request">The request object.</param>
        /// <param name="callback">The callback used to Continue or Cancel the request (async). </param>
        /// <returns>True if the request is being handled, false to cancel the request immediately. </returns>
        public bool ProcessRequest(IRequest request, ICallback callback)
        {
            if (_responseMessage != null)
            {
                throw new InvalidOperationException("Busy");
            }

            // Convert the CefSharp request to an ASP.NET request message. 
            using (HttpRequestMessage message = request.ToHttpRequestMessage())
            {
                // Ask the web API to handle the request. 
                _webAPI.HandleRequestAsync(message)
                    .ContinueWith(response =>
                    {
                        // Hold on to the response message. 
                        _responseMessage = response.Result;
                        callback.Continue();
                    }, TaskContinuationOptions.NotOnCanceled);
            }
            return true;
        }

        /// <summary>
        /// Retrieve response header information.
        /// </summary>
        /// <param name="response">Response object with properties to be set. </param>
        /// <param name="responseLength">Output response length. </param>
        /// <param name="redirectUrl">Output redirect URL. </param>
        public void GetResponseHeaders(IResponse response, out long responseLength, out string redirectUrl)
        {
            redirectUrl = null;
            if (_responseMessage == null)
            {
                // No response
                responseLength = 0;
                response.StatusCode = (int)System.Net.HttpStatusCode.NotFound;
                response.StatusText = "Not Found";
            }
            else
            {
                var content = _responseMessage.Content;
                if (content != null)
                {
                    // Response with content
                    response.MimeType = content.Headers.ContentType.MediaType;
                    if (response.MimeType.StartsWith("image/"))
                    {
                        _responseContent = content.ReadAsByteArrayAsync().Result;
                        _reponseOffset = 0;
#if DEBUG
                        //Console.WriteLine("GetResponseHeaders {0}", response.MimeType);
#endif
                    }
                    else
                    {
                        string responseJSON = content.ReadAsStringAsync().Result;
                        _responseContent = Encoding.UTF8.GetBytes(responseJSON);
                        _reponseOffset = 0;
#if DEBUG
                        //Console.WriteLine("GetResponseHeaders {0} {1}", response.MimeType, responseJSON);
#endif
                    }
                    responseLength = _responseContent.Length;
                }
                else
                {
                    // Response with no content (still has status)
                    _responseContent = null;
                    _reponseOffset = 0;
                    responseLength = 0;
                }

                // Status
                response.StatusCode = (int)_responseMessage.StatusCode;
                response.StatusText = _responseMessage.ReasonPhrase;
#if DEBUG
                //Console.WriteLine("GetResponseHeaders status = {0}", response.StatusText);
#endif
                _responseMessage.Dispose();
                _responseMessage = null;
            }
        }

        /// <summary>
        /// Read response data. 
        /// </summary>
        /// <param name="dataOut">Stream to write to. </param>
        /// <param name="bytesRead">Number of bytes copied to the stream. </param>
        /// <param name="callback">The callback used to Continue or Cancel the request (async). </param>
        /// <returns>True if there is response data, otherwise false. </returns>
        public bool ReadResponse(Stream dataOut, out int bytesRead, ICallback callback)
        {
            bytesRead = 0;

            // If there is response content, write it out. 
            if (_responseContent != null)
            {
                const int maxCefSharpStreamLength = 32768;
                bytesRead = Math.Min(_responseContent.Length - _reponseOffset, maxCefSharpStreamLength);
#if DEBUG
                //Console.WriteLine("ReadResponse bytesRead = {0}", bytesRead);
#endif
                dataOut.Write(_responseContent, _reponseOffset, bytesRead);
                _reponseOffset += bytesRead;

                if (_reponseOffset >= _responseContent.Length)
                {
                    // CefSharp uses Dispose to indicate that the request is completed. 
                    callback.Dispose();
                    _responseContent = null;
                }
            }
            else
            {
                // CefSharp uses Dispose to indicate that the request is completed. 
                callback.Dispose();
            }

            return bytesRead > 0;
        }

        /// <summary>
        /// Clean up. 
        /// </summary>
        public void Dispose()
        {
            if (_responseMessage != null)
            {
                _responseMessage.Dispose();
                _responseMessage = null;
            }
        }
    }
}
